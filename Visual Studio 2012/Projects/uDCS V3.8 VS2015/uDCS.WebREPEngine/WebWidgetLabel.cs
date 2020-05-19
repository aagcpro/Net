using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web.UI.WebControls;

using uDCS.REPEngine;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.XtraPivotGrid;

namespace uDCS.WebREPEngine
{
    public class WebWidgetLabel : WebWidgetBase
    {
        private Label m_pLabel = null;

        public WebWidgetLabel(XmlNode nodeWidget, eWidgetTypes eWidgetType, List<REPDataSet> pREPDataSets, Panel pREPPanel, WebREPEngine pREPEngine)
            : base(nodeWidget, eWidgetType, pREPDataSets, pREPPanel, pREPEngine)
        {
            m_pLabel = new Label();
        }

        public Label Label
        {
            get { return m_pLabel; }
        }

        public override void Show()
        {
            base.Show();

            XmlAttribute attrib = null;

            // Title
            if ( (attrib = m_nodeWidget.Attributes["Text"]) != null)
            {
                m_pLabel.Text = attrib.Value;
            }
            else if (m_pREPDataSet != null)
            {
                m_pLabel.Text = m_pREPDataSet.GetScalarResult().ToString();
            }
            else
            {
                throw new Exception("No Text AND REPDataSet defined for Label");
            }

            if ((attrib = m_nodeWidget.Attributes["Font.Name"]) != null)
            {
                m_pLabel.Font.Name = attrib.Value;
            }
            if ((attrib = m_nodeWidget.Attributes["Font.Size"]) != null)
            {
                m_pLabel.Font.Size = new FontUnit(attrib.Value);
            }
            if ((attrib = m_nodeWidget.Attributes["Font.Color"]) != null)
            {
                m_pLabel.ForeColor =  System.Drawing.Color.FromName(attrib.Value);
            }
            if ((attrib = m_nodeWidget.Attributes["Width"]) != null)
            {
                m_pLabel.Width = m_iWidth;
            }
            if ((attrib = m_nodeWidget.Attributes["Height"]) != null)
            {
                m_pLabel.Height = m_iHeight;
            }

            m_pWidgetPanel.Controls.Add(m_pLabel);
            m_pREPPanel.Controls.Add(m_pWidgetPanel);
        }
    }
}
