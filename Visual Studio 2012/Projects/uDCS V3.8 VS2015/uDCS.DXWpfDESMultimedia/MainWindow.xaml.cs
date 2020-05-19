using System;
using System.Linq;
using System.Windows;
using System.ComponentModel;

using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;

using uDCS.DESEngine;
using uDCS.WpfDESUtilities;
using uDCS.WpfDESEngine;

namespace uDCS.DXWpfDESMultimedia
{
    public partial class MainWindow : DXWindow
    {
        private bool m_bAskClosing = false;
        private static string s_uDESMultimediaVersion = "uDES-Multimedia: 3.2.2014-0806";

        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void DXRibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DevExpress.Xpf.Core.DXGridDataController.DisableThreadingProblemsDetection = true;

            try
            {
                m_teVersion.Text = s_uDESMultimediaVersion + FileUtils.CRLF + DEEngine.DEVERSION;
                Title = DBUtils.DSSettings.LocalFolder + " - " + DBUtils.DSUsers.PersonName + " [" + DBUtils.DSSettings.DeviceCode + "]";

                this.m_bAskClosing = true;

                m_pLayoutManager.DockItemClosing += m_pLayoutManager_DockItemClosing;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        private void DXRibbonWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.m_bAskClosing && MessageBox.Show("¿Realmente desea salir del sistema?", "AVISO", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.OK) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        void m_pLayoutManager_DockItemClosing(object sender, DevExpress.Xpf.Docking.Base.ItemCancelEventArgs e)
        {
            DocumentPanel pDocumentPanel = e.Item as DocumentPanel;
            if (pDocumentPanel != null)
            {
                DEControl pDEControl = pDocumentPanel.Content as DEControl;
                if (pDEControl != null)
                {
                    if (pDEControl.DEEngine.IsInEditionRunnigMode && !pDEControl.DEEngine.IsSaved)
                    {
                        if (MessageBox.Show("Realmente desea CANCELAR la entrevista?", "ADVERTENCIA", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel) == MessageBoxResult.Cancel)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        private void m_nviFormsCreate_Click(object sender, EventArgs e)
        {
            if (DBUtils.DSSettings == null || String.IsNullOrEmpty(DBUtils.DSSettings.LocalFolder))
            {
                MessageBox.Show("No se configurado este dispositivo");
                return;
            }
            QuestionnairesWindow pQuestionnairesWindow = new QuestionnairesWindow();
            pQuestionnairesWindow.Owner = this;
            bool? bResult = pQuestionnairesWindow.ShowDialog();
            if (bResult == true)
            {
                if (pQuestionnairesWindow.FullScreen)
                {
                    DEWindow pDEWindow = new DEWindow();
                    pDEWindow.DEControl.QuestionnaireCode = pQuestionnairesWindow.QuestionnaireCode;
                    pDEWindow.DEControl.RunningMode = eRunningModes.Executing;
                    pDEWindow.ShowDialog();
                }
                else
                {
                    DocumentPanel pDocumentPanel = new DocumentPanel();
                    pDocumentPanel.Caption = pQuestionnairesWindow.QuestionnaireCode;
                    m_dgDocuments.Add(pDocumentPanel);

                    DEControl pDEControl = new DEControl();
                    pDEControl.QuestionnaireCode = pQuestionnairesWindow.QuestionnaireCode;
                    pDEControl.ProcessId = DEEngine.PROCESSID_DATAENTRY;
                    pDEControl.RunningMode = DESEngine.eRunningModes.Executing;
                    pDEControl.OwnerWindow = this;
                    pDEControl.DocumentPanel = pDocumentPanel;
                    pDocumentPanel.Content = pDEControl;

                    pDEControl.RunSurvey();

                    m_dgDocuments.SelectedTabIndex = m_dgDocuments.Items.Count - 1;
                }
            }            
        }
        private void m_nviFormsSearch_Click(object sender, EventArgs e)
        {
            if (DBUtils.DSSettings == null || String.IsNullOrEmpty(DBUtils.DSSettings.LocalFolder))
            {
                MessageBox.Show("No se configurado este dispositivo");
                return;
            }
            InventoryWindow pInventoryWindow = new InventoryWindow();
            pInventoryWindow.Owner = this;
            bool? bResult = pInventoryWindow.ShowDialog();
            if (bResult == true)
            {
                DEControl pDEControl = null;
                DocumentPanel pDocumentPanel = null;
                switch (pInventoryWindow.SurveyAction)
                {
                    case eSurveyActions.Open:
                    case eSurveyActions.Execute:
                        pDocumentPanel = new DocumentPanel();
                        pDocumentPanel.Caption = pInventoryWindow.SurveyCode + " (Captura)";
                        m_dgDocuments.Add(pDocumentPanel);

                        pDEControl = new DEControl();
                        pDEControl.SurveyCode = pInventoryWindow.SurveyCode;
                        pDEControl.ProcessId = DEEngine.PROCESSID_DATAENTRY;
                        pDEControl.RunningMode = eRunningModes.Executing;
                        pDEControl.DocumentPanel = pDocumentPanel;
                        pDocumentPanel.Content = pDEControl;
                        pDEControl.RunSurvey();

                        m_dgDocuments.SelectedTabIndex = m_dgDocuments.Items.Count - 1;
                        break;

                    case eSurveyActions.Edit:
                        pDocumentPanel = new DocumentPanel();
                        pDocumentPanel.Caption = pInventoryWindow.SurveyCode + " (Edición)";
                        m_dgDocuments.Add(pDocumentPanel);

                        pDEControl = new DEControl();
                        pDEControl.SurveyCode = pInventoryWindow.SurveyCode;
                        pDEControl.ProcessId = DEEngine.PROCESSID_DATAENTRY;
                        pDEControl.RunningMode = eRunningModes.Editing;
                        pDEControl.DocumentPanel = pDocumentPanel;
                        pDocumentPanel.Content = pDEControl;
                        pDEControl.RunSurvey();

                        m_dgDocuments.SelectedTabIndex = m_dgDocuments.Items.Count - 1;
                        break;

                    case eSurveyActions.Supervise:
                        pDocumentPanel = new DocumentPanel();
                        pDocumentPanel.Caption = pInventoryWindow.SurveyCode + " (Supervisión)";
                        m_dgDocuments.Add(pDocumentPanel);

                        pDEControl = new DEControl();
                        pDEControl.SurveyCode = pInventoryWindow.SurveyCode;
                        pDEControl.ProcessId = DEEngine.PROCESSID_SUPERVISION;
                        pDEControl.RunningMode = eRunningModes.Editing;
                        pDEControl.DocumentPanel = pDocumentPanel;
                        pDocumentPanel.Content = pDEControl;
                        pDEControl.RunSurvey();

                        m_dgDocuments.SelectedTabIndex = m_dgDocuments.Items.Count - 1;
                        break;

                    case eSurveyActions.Review:
                        pDocumentPanel = new DocumentPanel();
                        pDocumentPanel.Caption = pInventoryWindow.SurveyCode + " (Revisión)";
                        m_dgDocuments.Add(pDocumentPanel);

                        pDEControl = new DEControl();
                        pDEControl.SurveyCode = pInventoryWindow.SurveyCode;
                        pDEControl.ProcessId = DEEngine.PROCESSID_DATAENTRY;
                        pDEControl.RunningMode = eRunningModes.Reviewing;
                        pDEControl.DocumentPanel = pDocumentPanel;
                        pDocumentPanel.Content = pDEControl;
                        pDEControl.RunSurvey();

                        m_dgDocuments.SelectedTabIndex = m_dgDocuments.Items.Count - 1;
                        break;

                    case eSurveyActions.Delete:
                    case eSurveyActions.Block:
                    case eSurveyActions.Unblock:
                        break;

                }
            }
        }
        private void m_nviFormsTest_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlgOpenFile = new Microsoft.Win32.OpenFileDialog();
            dlgOpenFile.DefaultExt = ".qst";
            dlgOpenFile.Filter = "Questionnaires (*.qst)|*.qst";
            Nullable<bool> result = dlgOpenFile.ShowDialog();
            if (result == true)
            {
                RunQuestionnaireAsWindow wndRunQuestAs = new RunQuestionnaireAsWindow();
                if ((bool)wndRunQuestAs.ShowDialog() == true)
                {
                    DocumentPanel pDocumentPanel = new DocumentPanel();
                    pDocumentPanel.Caption = dlgOpenFile.SafeFileName;
                    m_dgDocuments.Add(pDocumentPanel);

                    DEControl pDEControl = new DEControl();
                    pDEControl.FileName = dlgOpenFile.FileName;
                    pDEControl.ProcessId = DEEngine.PROCESSID_DATAENTRY;
                    pDEControl.RunningMode = wndRunQuestAs.RunningMode;
                    pDEControl.DocumentPanel = pDocumentPanel;
                    pDocumentPanel.Content = pDEControl;
                    pDEControl.RunSurvey();

                    m_dgDocuments.SelectedTabIndex = m_dgDocuments.Items.Count - 1;
                }
            }
        }
        private void m_nviSynchDownload_Click(object sender, EventArgs e)
        {
            if (DBUtils.DSSettings == null || String.IsNullOrEmpty(DBUtils.DSSettings.LocalFolder))
            {
                MessageBox.Show("No se configurado este dispositivo");
                return;
            }
            DownloadWindow pDownloadWindow = new DownloadWindow(false);
            pDownloadWindow.Owner = this;
            pDownloadWindow.ShowDialog();
        }
        private void m_nviSynchSend_Click(object sender, EventArgs e)
        {
            if (DBUtils.DSSettings == null || String.IsNullOrEmpty(DBUtils.DSSettings.LocalFolder))
            {
                MessageBox.Show("No se configurado este dispositivo");
                return;
            }
            UploadWindow pUploadWindow = new UploadWindow();
            pUploadWindow.Owner = this;
            pUploadWindow.ShowDialog(); 
        }
        private void m_nviSettingsServer_Click(object sender, EventArgs e)
        {
            ServerSetupWindow pServerSetupWindow = new ServerSetupWindow(false);
            pServerSetupWindow.Owner = this;
            pServerSetupWindow.ShowDialog();
            if (DBUtils.DSSettings != null && !String.IsNullOrEmpty(DBUtils.DSSettings.LocalFolder))
            {
                Title = DBUtils.DSSettings.LocalFolder;
            }
        }

    }
}
