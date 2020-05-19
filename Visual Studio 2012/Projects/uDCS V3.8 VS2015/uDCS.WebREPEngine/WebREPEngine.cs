using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;

using uDCS.REPEngine;

namespace uDCS.WebREPEngine
{
    public class WebREPEngine : REPEngine.REPEngine
    {
        private Panel m_pREPPanel = null;

        public WebREPEngine(string sReportCode, Panel pREPPanel)
            : base(sReportCode)
        {
            m_pREPPanel = pREPPanel;
        }
        public WebREPEngine(int iReportId, Panel pREPPanel)
            : base(iReportId)
        {
            m_pREPPanel = pREPPanel;
        }
        public Panel DEPanel
        {
            get { return m_pREPPanel; }
            set { m_pREPPanel = value; }
        }

        public override WidgetBase CreateWidgetTable(XmlNode nodeWidget, eWidgetTypes eWidgetType, List<REPDataSet> pREPDataSets)
        {
            WebWidgetTable webWidgetTable = new WebWidgetTable(nodeWidget, eWidgetType, pREPDataSets, m_pREPPanel, this);
            return webWidgetTable;
        }
        public override WidgetBase CreateWidgetGraph(XmlNode nodeWidget, eWidgetTypes eWidgetType, List<REPDataSet> pREPDataSets)
        {
            WebWidgetGraph webWidgetGraph = new WebWidgetGraph(nodeWidget, eWidgetType, pREPDataSets, m_pREPPanel, this);
            return webWidgetGraph;
        }
        public override WidgetBase CreateWidgetLabel(XmlNode nodeWidget, eWidgetTypes eWidgetType, List<REPDataSet> pREPDataSets)
        {
            WebWidgetLabel webWidgetLabel = new WebWidgetLabel(nodeWidget, eWidgetType, pREPDataSets, m_pREPPanel, this);
            return webWidgetLabel;
        }
    }
}
