using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using AxDSOFramer;
using System.Xml;
using System.ComponentModel;
using System.Windows.Threading;

namespace OfficeDocumentHost
{
    public partial class DocumentWindow : Window
    {

        public DocumentWindow()
        {
            InitializeComponent();
        }

        #region CloseDocument

        public static RoutedUICommand CloseDocument = new RoutedUICommand("Close Document", "CloseDocument", typeof(DocumentWindow));

        private void CloseDocumentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // if this were a real app, we might prompt the user to save unsaved changes...
            // but we're lazy so we'll just ask if they really want to close the current document
            if (System.Windows.MessageBox.Show("Do you really want to close '" + _documentName + "'?", "Close Document?",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                try
                {
                    framer.Close();
                    _documentName = string.Empty;
                    Title = "WPF Office Document Host";
                }
                catch (Exception) { }
            }
        }

        private void CanCloseDocument(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(_documentName);
        }

        #endregion

        #region CreateDocument

        public static RoutedUICommand CreateDocument = new RoutedUICommand("Create Document", "CreateDocument", typeof(DocumentWindow));

        private void CreateDocumentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                framer.CreateNew(OfficeProgram.SelectedValue as string);
                _documentName = (OfficeProgram.SelectedItem as XmlElement).Attributes["UntitledDocumentName"].Value;
                Title = "WPF Office Document Host - " + _documentName;
                _isNewDocument = true;
            }
            catch (Exception) 
            {
                System.Windows.MessageBox.Show("Unable to create document.",
                    "Create Document Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
            }
        }

        private void CanCreateDocument(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = string.IsNullOrEmpty(_documentName);
        }

        #endregion

        #region OpenDocument

        public static RoutedUICommand OpenDocument = new RoutedUICommand("Open Document", "OpenDocument", typeof(DocumentWindow));

        private void OpenDocumentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // we're already linking with WinForms, so for simplicity, we'll just
            // leverage the WinForms OpenFileDialog
            if (_openFileDialog == null)
            {
                _openFileDialog = new OpenFileDialog();
                _openFileDialog.Filter = "Microsoft Office Files|*.doc;*.docx;*.docm;*.rtf;*.xls;*.xlsx;*.xlsm;*.xlsb;*.csv;*.ppt;*.pptx;*.pptm;*.vsd;*.vdx|All Files|*.*";
            }
            DialogResult result = _openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    framer.Select();
                    framer.Open(_openFileDialog.FileName);
                    _documentName = framer.DocumentName;
                    Title = "WPF Office Document Host - " + _documentName;
                    _isNewDocument = false;
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Unable to open document: " + _openFileDialog.FileName, 
                        "Open Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
                }
            }
        }

        private void CanOpenDocument(object sender, CanExecuteRoutedEventArgs e)
        {
            // require user to close current document before opening a new one
            e.CanExecute = string.IsNullOrEmpty(_documentName);
        }

        #endregion

        #region SaveDocument

        public static RoutedUICommand SaveDocument = new RoutedUICommand("Save Document", "SaveDocument", typeof(DocumentWindow));

        private void SaveDocumentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                framer.Save();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Unable to save document '" + _documentName + "'. " + ex.Message,
                    "Save Document Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
            }
        }

        private void CanSaveDocument(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(_documentName) && !_isNewDocument;
        }

        #endregion

        #region SaveDocumentAs

        public static RoutedUICommand SaveDocumentAs = new RoutedUICommand("Save Document As", "SaveDocumentAs", typeof(DocumentWindow));

        private void SaveDocumentAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // we're already linking with WinForms, so for simplicity, we'll just
            // leverage the WinForms SaveFileDialog
            if (_saveFileDialog == null)
            {
                _saveFileDialog = new SaveFileDialog();
                _saveFileDialog.OverwritePrompt = true;
            }
            _saveFileDialog.FileName = _documentName;
            _saveFileDialog.DefaultExt = System.IO.Path.GetExtension(_documentName);
            DialogResult result = _saveFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    framer.Save(_saveFileDialog.FileName, true, null, null);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Unable to save document as '" + _saveFileDialog.FileName + "'. " + ex.Message,
                        "Save Document Failed", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
                }
            }
        }

        private void CanSaveDocumentAs(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(_documentName);
        }

        #endregion

        private void InitializeFramerControl(object sender, RoutedEventArgs e)
        {
            // We set certain DSO Framer properties in response to 
            // the Loaded event because we need to ensure that the
            // framer control is connected to its parent (added to 
            // its Controls collection) prior to setting properties 
            // like Titlebar, Menubar, etc. Attempting to set these 
            // properties before the control is parented will result
            // in an InvalidActiveXStateException.
            framer.Menubar = false;
            framer.Titlebar = false;
        }

        private void DocumentOpened(object sender, _DFramerCtlEvents_OnDocumentOpenedEvent e)
        {
            // When a new document is opened, update the selection of the combobox
            // to match the document type. The following approach is not necessarily
            // accurate since it makes assumptions on the document type based on its
            // file extension. It also assumes that the office programs are listed 
            // alphabetically in the OfficePrograms XmlDataProvider.  But its good
            // enough for demo purposes.
            switch (System.IO.Path.GetExtension(e.file).ToLower())
            {
                case ".xls":
                case ".xlsx":
                case ".xlsm":
                case ".xlsb":
                case ".csv":
                    OfficeProgram.SelectedIndex = 0;
                    break;

                case ".ppt":
                case ".pptx":
                case ".pptm":
                    OfficeProgram.SelectedIndex = 1;
                    break;

                case ".vsd":
                case ".vdx":
                    OfficeProgram.SelectedIndex = 2;
                    break;

                case ".doc":
                case ".docx":
                case ".docm":
                case ".rtf":
                    OfficeProgram.SelectedIndex = 3;
                    break;
            }
        }

        private void SaveCompleted(object sender, _DFramerCtlEvents_OnSaveCompletedEvent e)
        {
            _isNewDocument = false;
            _documentName = e.docName;
            Title = "WPF Office Document Host - " + _documentName;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_documentName))
            {
                if (System.Windows.MessageBox.Show("Do you really want to close '" + _documentName + "'?", "Close Document?",
                   MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
           }
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // We need to reactivate the ActiveX control after the window size changes.
            // There are probably lots of other places where we need to reactivate the
            // control, but we're lazy.
            _isFramerDirty = true;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate(object o)
            {
                if (framer != null && _isFramerDirty)
                {
                    framer.Activate();
                }
                return null;
            }, null);
            framer.Activate();
        }

        private string _documentName = string.Empty;
        private bool _isNewDocument = false;
        private bool _isFramerDirty = false;
        private OpenFileDialog _openFileDialog = null;
        private SaveFileDialog _saveFileDialog = null;
    }
}