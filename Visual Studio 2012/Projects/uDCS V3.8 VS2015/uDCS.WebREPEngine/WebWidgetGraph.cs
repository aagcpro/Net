using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web.UI.WebControls;

using uDCS.REPEngine;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;

namespace uDCS.WebREPEngine
{
    public class WebWidgetGraph : WebWidgetBase
    {
        private WebChartControl m_xwcWebChart = null;
        private List<GraphSerie> m_pGraphSeries = null;
        private eGraphTypes m_eGraphType = eGraphTypes.NotDefined;
        private Boolean m_bLabelVisible = true;

        public WebWidgetGraph(XmlNode nodeWidget, eWidgetTypes eWidgetType, List<REPDataSet> pREPDataSets, Panel pREPPanel, WebREPEngine pREPEngine)
            : base(nodeWidget, eWidgetType, pREPDataSets, pREPPanel, pREPEngine)
        {
            m_xwcWebChart = new WebChartControl();
            m_xwcWebChart.EnableViewState = false;
            m_xwcWebChart.CustomDrawSeries += new CustomDrawSeriesEventHandler(Chart_CustomDrawSeries);
            m_xwcWebChart.BoundDataChanged +=new BoundDataChangedEventHandler(Chart_BoundDataChanged);
        }

        public WebChartControl WebChart
        {
            get { return m_xwcWebChart; }
        }

        public override void Show()
        {
            base.Show();
            if (m_pREPDataSet == null)
            {
                throw new Exception("REPDataSet not defined");
            }

            XmlAttribute attrib = null;
            ViewType eViewType = ViewType.Bar;

            if ((attrib = m_nodeWidget.Attributes["Title"]) != null)
            {
                Label pLabel = new Label();
                pLabel.Text = attrib.Value + "<br>";

                m_pWidgetPanel.Controls.Add(pLabel);
            }

            // Graph type
            if ((attrib = m_nodeWidget.Attributes["Type"]) != null)
            {

                m_eGraphType = (eGraphTypes)Enum.Parse(typeof(eGraphTypes), attrib.Value);
                switch (m_eGraphType)
                {   
                    case eGraphTypes.Pie:
                        eViewType = ViewType.Pie;
                        break;
                    case eGraphTypes.Line:
                        eViewType = ViewType.Line;
                        break;
                    case eGraphTypes.Stacked:
                        eViewType = ViewType.StackedBar;
                        break;
                    case eGraphTypes.FullStacked:
                        eViewType = ViewType.FullStackedBar;
                        break;
                    case eGraphTypes.Bar:
                        eViewType = ViewType.Bar;
                        break;
                    case eGraphTypes.Bubble:
                        eViewType = ViewType.Bubble;
                        break;
                    case eGraphTypes.Point:
                        eViewType = ViewType.Point;
                        break;
                    case eGraphTypes.Stock:
                        eViewType = ViewType.Stock;
                        break;

                    // 3D
                    case eGraphTypes.Pie3D:
                        eViewType = ViewType.Pie3D;
                        break;
                    case eGraphTypes.Line3D:
                        eViewType = ViewType.Line3D;
                        break;
                    case eGraphTypes.Stacked3D:
                        eViewType = ViewType.StackedBar3D;
                        break;
                    case eGraphTypes.FullStacked3D:
                        eViewType = ViewType.FullStackedBar3D;
                        break;
                    case eGraphTypes.Bar3D:
                        eViewType = ViewType.Bar3D;
                        break;
                }
                m_xwcWebChart.SeriesTemplate.ChangeView(eViewType);
            }

            m_pWidgetPanel.Controls.Add(m_xwcWebChart);
            m_pREPPanel.Controls.Add(m_pWidgetPanel);

            m_xwcWebChart.DataSource = m_pREPDataSet.GetDataTable();
            m_xwcWebChart.DataBind();

            m_xwcWebChart.BackColor = BackColor;
            System.Drawing.Font pFont = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Bold); 

            // Configuración especial para la gráfica tipo Point.
            if (this.m_eGraphType == eGraphTypes.Point)
            {
                Series CurrentSerie;
                System.Data.DataTable dtData = m_pREPDataSet.GetDataTable();
                m_xwcWebChart.Series.Clear();
                String sSerie = "", sCodeX = "", sCodeY = "";

                foreach (XmlNode nodeWidget in m_nodeWidget.ChildNodes)
                {
                    switch (nodeWidget.Name)
                    {
                        case "Serie":
                            if ((attrib = nodeWidget.Attributes["Code"]) == null)
                                throw new Exception("No Code defined for Serie Graph");
                            sSerie = attrib.Value;
                            break;
                        case "Value":
                            if ((attrib = nodeWidget.Attributes["CodeX"]) == null)
                                throw new Exception("No Code X defined for Serie Graph");

                            sCodeX = attrib.Value;

                            if ((attrib = nodeWidget.Attributes["CodeY"]) == null)
                                throw new Exception("No Code Y defined for Serie Graph");

                            sCodeY = attrib.Value;
                            break;
                    }
                }

                for (int iIndex = 0; iIndex < dtData.Rows.Count; iIndex++)
                {
                    CurrentSerie = new Series(dtData.Rows[iIndex][sSerie].ToString(), ViewType.Point);
                    CurrentSerie.ArgumentScaleType = ScaleType.Numerical;
                    CurrentSerie.Points.Add(new SeriesPoint(dtData.Rows[iIndex][sCodeX], dtData.Rows[iIndex][sCodeY]));
                    m_xwcWebChart.Series.Add(CurrentSerie);

                    PointSeriesView pPointSeriesView = (PointSeriesView)CurrentSerie.View;
                    pPointSeriesView.PointMarkerOptions.Kind = MarkerKind.Circle;
                    //pPointSeriesView.PointMarkerOptions.StarPointCount = 10;
                    pPointSeriesView.PointMarkerOptions.Size = 20;
                }

                XYDiagram diagram = m_xwcWebChart.Diagram as XYDiagram;

                //Esto es sólo para C&A
                ConstantLine clX = new ConstantLine("X", "3");
                ConstantLine clY = new ConstantLine("Y", "3");
                diagram.AxisX.ConstantLines.Add(clX);
                diagram.AxisY.ConstantLines.Add(clY);

                clX.Title.Visible = false;
                clX.LineStyle.DashStyle = DashStyle.Dash;
                clX.ShowInLegend = false;

                clY.Title.Visible = false;
                clY.LineStyle.DashStyle = DashStyle.Dash;
                clY.ShowInLegend = false;
                //Esto es sólo para C&A
            }

            foreach (XmlNode nodeWidget in m_nodeWidget.ChildNodes)
            {
                switch (nodeWidget.Name)
                {
                    case "Serie":
                        if (this.m_eGraphType != eGraphTypes.Point)
                        {
                            if ((attrib = nodeWidget.Attributes["Code"]) == null)
                                throw new Exception("No Code defined for Serie Graph");
                            m_xwcWebChart.SeriesDataMember = attrib.Value;
                        }
                        break;
                    case "Series":
                        m_pGraphSeries = new List<GraphSerie>();

                        if ((attrib = nodeWidget.Attributes["Label.Visible"]) != null)
                            this.m_bLabelVisible = Convert.ToBoolean(attrib.Value);

                        foreach (XmlNode seriesNode in nodeWidget)
                        {
                            string sName = "", sColor = "";
                            GraphSerie pGraphSerie = null;
                            if ((attrib = seriesNode.Attributes["Name"]) == null)
                                throw new Exception("No Name defined for Series Graph");
                            sName = attrib.Value;
                            if ((attrib = seriesNode.Attributes["Color"]) == null)
                                throw new Exception("No Color defined for Series Graph");
                            sColor = attrib.Value;
                            pGraphSerie = new GraphSerie(sName, sColor);
                            if ((attrib = seriesNode.Attributes["Type"]) != null)
                                pGraphSerie.eSerieType = (eGraphTypes)Enum.Parse(typeof(eGraphTypes), attrib.Value);
                            else
                                pGraphSerie.eSerieType = m_eGraphType;

                            m_pGraphSeries.Add(pGraphSerie);
                        }
                        break;
                    case "Axis":
                        // Y Axis
                        ((XYDiagram)m_xwcWebChart.Diagram).DefaultPane.BackColor = BackColor;
                        ((XYDiagram)m_xwcWebChart.Diagram).AxisY.GridLines.Color = System.Drawing.Color.WhiteSmoke;
                        ((XYDiagram)m_xwcWebChart.Diagram).AxisY.Label.Font = pFont;
                        ((XYDiagram)m_xwcWebChart.Diagram).AxisY.VisualRange.Auto = false;

                        if ((attrib = nodeWidget.Attributes["AxisY.Title.Text"]) != null)
                        {
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.Title.Visible = true;
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.Title.Text = attrib.Value;
                        }
                        if ((attrib = nodeWidget.Attributes["AxisY.Visible"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.Visible = Convert.ToBoolean(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisY.GridLines.Visible"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.GridLines.Visible = Convert.ToBoolean(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisY.Reverse"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.Reverse = Convert.ToBoolean(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisY.Range.MinValue"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.VisualRange.MinValue = Convert.ToDecimal(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisY.Range.MaxValue"]) != null)
                        {
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.WholeRange.MaxValue = Convert.ToDecimal(attrib.Value);
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.VisualRange.MaxValue = Convert.ToDecimal(attrib.Value);
                        }
                        if ((attrib = nodeWidget.Attributes["AxisY.GridSpacing"]) != null)
                        {
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.NumericScaleOptions.AutoGrid = false;
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.NumericScaleOptions.GridSpacing = Convert.ToDouble(attrib.Value);
                        }
                        if ((attrib = nodeWidget.Attributes["AxisY.Label.Staggered"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.Label.Staggered = Convert.ToBoolean(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisY.NumericOptions.Format"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisY.Label.TextPattern = "{V:" + attrib.Value + "}";

                        // X Axis
                        ((XYDiagram)m_xwcWebChart.Diagram).AxisX.Label.Font = pFont;
                        ((XYDiagram)m_xwcWebChart.Diagram).AxisX.VisualRange.Auto = false;

                        if ((attrib = nodeWidget.Attributes["AxisX.Title.Text"]) != null)
                        {
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.Title.Visible = true;
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.Title.Text = attrib.Value;
                        }
                        if ((attrib = nodeWidget.Attributes["AxisX.Visible"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.Visible = Convert.ToBoolean(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisX.GridLines.Visible"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.GridLines.Visible = Convert.ToBoolean(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisX.Reverse"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.Reverse = Convert.ToBoolean(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisX.Range.MinValue"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.VisualRange.MinValue = Convert.ToDecimal(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisX.Range.MaxValue"]) != null)
                        {
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.WholeRange.MaxValue = Convert.ToDecimal(attrib.Value);
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.VisualRange.MaxValue = Convert.ToDecimal(attrib.Value);
                        }
                        if ((attrib = nodeWidget.Attributes["AxisX.GridSpacing"]) != null)
                        {
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.NumericScaleOptions.GridSpacing = Convert.ToDouble(attrib.Value);
                        }
                        if ((attrib = nodeWidget.Attributes["AxisX.Label.Staggered"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.Label.Staggered = Convert.ToBoolean(attrib.Value);
                        if ((attrib = nodeWidget.Attributes["AxisX.NumericOptions.Format"]) != null)
                            ((XYDiagram)m_xwcWebChart.Diagram).AxisX.Label.TextPattern = "{V:" + attrib.Value + "}";
                        break;
                    case "Value":
                        if (this.m_eGraphType != eGraphTypes.Point)
                        {
                            if ((attrib = nodeWidget.Attributes["Code"]) == null)
                                throw new Exception("No Value defined for Serie Graph");

                            switch (eViewType)
                            {
                                case ViewType.Bubble:
                                    XmlAttribute attribWeight = nodeWidget.Attributes["CodeWeight"];
                                    m_xwcWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { attrib.Value, attribWeight.Value });
                                    break;
                                default:
                                    m_xwcWebChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { attrib.Value });
                                    break;
                            }
                        }

                        if ((attrib = nodeWidget.Attributes["Format"]) != null)
                        {
                            m_xwcWebChart.SeriesTemplate.Label.TextPattern = "{V:" + attrib.Value + "}";
                            m_xwcWebChart.SeriesTemplate.CrosshairLabelPattern = "{V:" + attrib.Value + "}";
                        }
                        break;
                    case "Argument":
                        if ((attrib = nodeWidget.Attributes["Code"]) == null)
                            throw new Exception("No Argument defined for Serie Graph");
                        m_xwcWebChart.SeriesTemplate.ArgumentDataMember = attrib.Value;
                        break;

                    case "Legend":
                        if ((attrib = nodeWidget.Attributes["Visible"]) != null)
                            m_xwcWebChart.Legend.Visible = Convert.ToBoolean(attrib.Value);

                        if (m_xwcWebChart.Legend.Visible)
                        {
                            m_xwcWebChart.Legend.BackColor = BackColor;
                            m_xwcWebChart.Legend.MarkerSize = new System.Drawing.Size(14, 14);
                            m_xwcWebChart.Legend.FillStyle.FillMode = FillMode.Solid;
                            m_xwcWebChart.Legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
                            m_xwcWebChart.Legend.Font = pFont;

                            if ((attrib = nodeWidget.Attributes["HorizontalPosition"]) != null)
                            {
                                LegendAlignmentHorizontal eLegendAligmentHorizontal = LegendAlignmentHorizontal.Center;

                                eLegendPositions eLegendPosition = (eLegendPositions)Enum.Parse(typeof(eLegendPositions), attrib.Value);
                                switch (eLegendPosition)
                                {
                                    case eLegendPositions.Center:
                                        eLegendAligmentHorizontal = LegendAlignmentHorizontal.Center;
                                        break;
                                    case eLegendPositions.Left:
                                        eLegendAligmentHorizontal = LegendAlignmentHorizontal.Left;
                                        break;
                                    case eLegendPositions.Right:
                                        eLegendAligmentHorizontal = LegendAlignmentHorizontal.Right;
                                        break;
                                    default:
                                        throw new Exception("Horizontal Position for Legend not suported");
                                        break;
                                }
                                m_xwcWebChart.Legend.AlignmentHorizontal = eLegendAligmentHorizontal;
                            }

                            if ((attrib = nodeWidget.Attributes["VerticalPosition"]) != null)
                            {
                                LegendAlignmentVertical eLegendAligmentVertical = LegendAlignmentVertical.Center;

                                eLegendPositions eLegendPosition = (eLegendPositions)Enum.Parse(typeof(eLegendPositions), attrib.Value);
                                switch (eLegendPosition)
                                {
                                    case eLegendPositions.Center:
                                        eLegendAligmentVertical = LegendAlignmentVertical.Center;
                                        break;
                                    case eLegendPositions.Bottom:
                                        eLegendAligmentVertical = LegendAlignmentVertical.BottomOutside;
                                        break;
                                    case eLegendPositions.Top:
                                        eLegendAligmentVertical = LegendAlignmentVertical.TopOutside;
                                        break;
                                    default:
                                        throw new Exception("Vertical Position for Legend not suported");
                                        break;
                                }
                                m_xwcWebChart.Legend.AlignmentVertical = eLegendAligmentVertical;
                            }
                            if ((attrib = nodeWidget.Attributes["Direction"]) != null)
                            {
                                eLegendDirections eLegendDirection = (eLegendDirections)Enum.Parse(typeof(eLegendDirections), attrib.Value);
                                LegendDirection pLegendDirection = LegendDirection.BottomToTop;
                                switch (eLegendDirection)
                                {
                                    case eLegendDirections.BottomToTop:
                                        pLegendDirection = LegendDirection.BottomToTop;
                                        break;
                                    case eLegendDirections.LeftToRight:
                                        pLegendDirection = LegendDirection.LeftToRight;
                                        break;
                                    case eLegendDirections.RightToLeft:
                                        pLegendDirection = LegendDirection.RightToLeft;
                                        break;
                                    case eLegendDirections.TopToBotom:
                                        pLegendDirection = LegendDirection.TopToBottom;
                                        break;
                                }

                                m_xwcWebChart.Legend.Direction = pLegendDirection;
                            }
                        }   // FIN if (m_xwcWebChart.Legend.Visible)
                        break;
                }
            }

            m_xwcWebChart.SeriesTemplate.ValueScaleType = ScaleType.Numerical;
            m_xwcWebChart.RefreshData();

            // Graph Orientation
            if ((attrib = m_nodeWidget.Attributes["Orientation"]) != null)
            {
                eGraphOrientation eGraphOrientation = (eGraphOrientation)Enum.Parse(typeof(eGraphOrientation), attrib.Value);
                if (eGraphOrientation == eGraphOrientation.Horizontal)
                    ((DevExpress.XtraCharts.XYDiagram)m_xwcWebChart.Diagram).Rotated = true;
            }

            ((XYDiagram)m_xwcWebChart.Diagram).DefaultPane.BorderVisible = false;

            // Aditional properties
            m_xwcWebChart.Width = m_iWidth;
            m_xwcWebChart.Height = m_iHeight;

            // Configuración adicional para la Gráfica tipo Bubble. 
            if (this.m_eGraphType == eGraphTypes.Bubble)
            {
                foreach (Series pSerie in m_xwcWebChart.Series)
                {
                    double dMinValue = Convert.ToDouble(pSerie.Points[0].Values.GetValue(1));
                    double dMaxValue = 0;

                    ((BubbleSeriesView)pSerie.View).MaxSize = 1;
                    ((BubbleSeriesView)pSerie.View).MinSize = 0.1;

                    for (int iPoint = 0; iPoint < pSerie.Points.Count; iPoint++)
                    {
                        if (dMaxValue < Convert.ToDouble(pSerie.Points[iPoint].Values.GetValue(1)))
                            dMaxValue = Convert.ToDouble(pSerie.Points[iPoint].Values.GetValue(1));

                        if (dMinValue > Convert.ToDouble(pSerie.Points[iPoint].Values.GetValue(1)))
                            dMinValue = Convert.ToDouble(pSerie.Points[iPoint].Values.GetValue(1));
                    }

                    pSerie.Label.Visible = false;

                    if(m_xwcWebChart.Series.Count < 7)
                        ((BubbleSeriesView)pSerie.View).AxisX.Range.MaxValueInternal = 7;

                    String sExp = "1." + m_xwcWebChart.Series.Count.ToString();

                    dMaxValue = dMaxValue * Convert.ToDouble(sExp);
                    dMinValue = dMinValue * Convert.ToDouble(sExp);

                    if (dMaxValue <= dMinValue) dMinValue = dMaxValue - 0.01;

                    ((BubbleSeriesView)pSerie.View).MaxSize = dMaxValue;
                    ((BubbleSeriesView)pSerie.View).MinSize = dMinValue;
                    //((BubbleSeriesView)pSerie.View).MaxSize = 0.2;
                    //((BubbleSeriesView)pSerie.View).MinSize = 0.1;
                    //((BubbleSeriesView)xwcWebChart.Series[iIndex].View).BubbleMarkerOptions.Kind = MarkerKind.Cross;

                    //((BubbleSeriesView)pSerie.View).Transparency = 100;
                }
            }

        }

        protected void Chart_CustomDrawSeries(object sender, DevExpress.XtraCharts.CustomDrawSeriesEventArgs e)
        {
            if (m_pGraphSeries != null)
            {
                e.Series.LabelsVisibility = (this.m_bLabelVisible) ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;

                foreach (GraphSerie pSerie in m_pGraphSeries)
                {
                    if (pSerie.Name == e.Series.Name)
                    {
                        switch (pSerie.eSerieType)
                        {
                            case eGraphTypes.Pie:
                                DevExpress.XtraCharts.PieDrawOptions pieOptions = e.SeriesDrawOptions as DevExpress.XtraCharts.PieDrawOptions;
                                DevExpress.XtraCharts.PieDrawOptions lpieOptions = e.LegendDrawOptions as DevExpress.XtraCharts.PieDrawOptions;
                                if (pieOptions != null)
                                {
                                    pieOptions.Color = pSerie.Color;
                                    pieOptions.FillStyle.FillMode = FillMode.Solid;

                                    lpieOptions.Color = pSerie.Color;
                                    lpieOptions.FillStyle.FillMode = FillMode.Solid;
                                }
                                break;
                            case eGraphTypes.Point:
                                DevExpress.XtraCharts.PointDrawOptions Point = e.SeriesDrawOptions as DevExpress.XtraCharts.PointDrawOptions;
                                DevExpress.XtraCharts.PointDrawOptions lPoint = e.LegendDrawOptions as DevExpress.XtraCharts.PointDrawOptions;
                                if (Point != null)
                                {
                                    Point.Color = pSerie.Color;
                                    Point.Shadow.Visible = true;
                                    Point.Marker.BorderColor = pSerie.Color;
                                    Point.Marker.FillStyle.FillMode = FillMode.Gradient;
                                    ((GradientFillOptionsBase)Point.Marker.FillStyle.Options).Color2 = System.Drawing.Color.White;

                                    lPoint.Color = pSerie.Color;
                                    lPoint.Shadow.Visible = true;
                                    lPoint.Marker.BorderColor = pSerie.Color;
                                    lPoint.Marker.FillStyle.FillMode = FillMode.Gradient;
                                    ((GradientFillOptionsBase)lPoint.Marker.FillStyle.Options).Color2 = System.Drawing.Color.White;
                                }
                                break;
                            case eGraphTypes.Line:
                                DevExpress.XtraCharts.LineDrawOptions lineOptions = e.SeriesDrawOptions as DevExpress.XtraCharts.LineDrawOptions;
                                DevExpress.XtraCharts.LineDrawOptions llineOptions = e.LegendDrawOptions as DevExpress.XtraCharts.LineDrawOptions;
                                if (lineOptions != null)
                                {
                                    lineOptions.Color = pSerie.Color;
                                    lineOptions.Marker.BorderColor = pSerie.Color;
                                    lineOptions.Marker.Color = pSerie.Color;
                                    lineOptions.Marker.FillStyle.FillMode = FillMode.Solid;

                                    llineOptions.Color = pSerie.Color;
                                    llineOptions.Marker.BorderColor = pSerie.Color;
                                    llineOptions.Marker.Color = pSerie.Color;
                                    llineOptions.Marker.FillStyle.FillMode = FillMode.Solid;
                                }
                                break;
                            case eGraphTypes.Stacked:
                            case eGraphTypes.FullStacked:
                            case eGraphTypes.Bar:
                                DevExpress.XtraCharts.BarDrawOptions barOptions = e.SeriesDrawOptions as DevExpress.XtraCharts.BarDrawOptions;
                                DevExpress.XtraCharts.BarDrawOptions lbarOptions = e.LegendDrawOptions as DevExpress.XtraCharts.BarDrawOptions;

                                if (barOptions != null)
                                {
                                    barOptions.Color = pSerie.Color;
                                    barOptions.Shadow.Visible = true;
                                    barOptions.FillStyle.FillMode = FillMode.Solid;
                                    //((RectangleGradientFillOptions)barOptions.FillStyle.Options).Color2 = System.Drawing.Color.DarkGray;
                                    //((RectangleGradientFillOptions)barOptions.FillStyle.Options).GradientMode = RectangleGradientMode.RightToLeft;

                                    lbarOptions.Color = pSerie.Color;
                                    lbarOptions.Shadow.Visible = true;
                                    lbarOptions.FillStyle.FillMode = FillMode.Solid;
                                    //((RectangleGradientFillOptions)lbarOptions.FillStyle.Options).Color2 = System.Drawing.Color.DarkGray;
                                    //((RectangleGradientFillOptions)lbarOptions.FillStyle.Options).GradientMode = RectangleGradientMode.RightToLeft;
                                }
                                if (m_eGraphType == eGraphTypes.FullStacked)
                                {
                                    e.Series.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                                    e.Series.PointOptions.ValueNumericOptions.Precision = 0;
                                }
                                break;
                            case eGraphTypes.Bubble:
                                DevExpress.XtraCharts.PointDrawOptionsBase pointOptions = e.SeriesDrawOptions as DevExpress.XtraCharts.PointDrawOptionsBase;
                                DevExpress.XtraCharts.PointDrawOptionsBase lpointOptions = e.LegendDrawOptions as DevExpress.XtraCharts.PointDrawOptionsBase;

                                if (pointOptions != null)
                                {
                                    pointOptions.Color = pSerie.Color;
                                    pointOptions.Shadow.Visible = true;
                                    pointOptions.Marker.FillStyle.FillMode = FillMode.Gradient;

                                    ((GradientFillOptionsBase)pointOptions.Marker.FillStyle.Options).Color2 = System.Drawing.Color.White;

                                    lpointOptions.Color = pSerie.Color;
                                    lpointOptions.Shadow.Visible = true;
                                    lpointOptions.Marker.FillStyle.FillMode = FillMode.Gradient;
                                    ((GradientFillOptionsBase)lpointOptions.Marker.FillStyle.Options).Color2 = System.Drawing.Color.White;
                                }
                                break;
                            case eGraphTypes.Stock:
                                DevExpress.XtraCharts.StockDrawOptions stockOptions = e.SeriesDrawOptions as DevExpress.XtraCharts.StockDrawOptions;
                                DevExpress.XtraCharts.StockDrawOptions lstockOptions = e.LegendDrawOptions as DevExpress.XtraCharts.StockDrawOptions;

                                if (stockOptions != null)
                                {
                                    stockOptions.Color = pSerie.Color;
                                    stockOptions.Shadow.Color = pSerie.Color;
                                    stockOptions.LevelLineLength = 0.5;
                                    stockOptions.ShowOpenClose = StockType.Both;
                                    stockOptions.LineThickness = 3;

                                    lstockOptions.Color = pSerie.Color;
                                    lstockOptions.Shadow.Color = pSerie.Color;
                                }
                                break;
                            // 3D
                            case eGraphTypes.Pie3D:
                                //drawOptions = e.SeriesDrawOptions as DevExpress.XtraCharts.Pie3DDrawOptions;
                                break;
                            case eGraphTypes.Line3D:
                                //drawOptions = e.SeriesDrawOptions as DevExpress.XtraCharts.Line3DDrawOptions;
                                break;
                            case eGraphTypes.Stacked3D:
                            case eGraphTypes.FullStacked3D:
                            case eGraphTypes.Bar3D:
                                //drawOptions = e.SeriesDrawOptions as DevExpress.XtraCharts.Bar3DDrawOptions;
                                break;
                        }   // END switch (pSerie.eSerieType)
                    }   // END if (pSerie.Name == e.Series.Name)
                }   // END foreach (GraphSerie pSerie in m_pGraphSeries)
            }
        }
        protected void Chart_BoundDataChanged(object sender, EventArgs e)
        {
            if (m_pGraphSeries != null)
            {
                foreach (Series pSerieGraph in m_xwcWebChart.Series)
                {
                    foreach (GraphSerie pSerie in m_pGraphSeries)
                    {
                        if (pSerie.Name == pSerieGraph.Name)
                        {
                            if (pSerie.eSerieType != m_eGraphType)
                            {
                                ViewType eViewType = ViewType.Bar;

                                switch (pSerie.eSerieType)
                                {
                                    case eGraphTypes.Pie:
                                        eViewType = ViewType.Pie;
                                        break;
                                    case eGraphTypes.Line:
                                        eViewType = ViewType.Line;
                                        break;
                                    case eGraphTypes.Stacked:
                                        eViewType = ViewType.StackedBar;
                                        break;
                                    case eGraphTypes.FullStacked:
                                        eViewType = ViewType.FullStackedBar;
                                        break;
                                    case eGraphTypes.Bar:
                                        eViewType = ViewType.Bar;
                                        break;
                                    case eGraphTypes.Point:
                                        eViewType = ViewType.Point;
                                        break;
                                    case eGraphTypes.Stock:
                                        eViewType = ViewType.Stock;
                                        break;

                                    // 3D
                                    case eGraphTypes.Pie3D:
                                        eViewType = ViewType.Pie3D;
                                        break;
                                    case eGraphTypes.Line3D:
                                        eViewType = ViewType.Line3D;
                                        break;
                                    case eGraphTypes.Stacked3D:
                                        eViewType = ViewType.StackedBar3D;
                                        break;
                                    case eGraphTypes.FullStacked3D:
                                        eViewType = ViewType.FullStackedBar3D;
                                        break;
                                    case eGraphTypes.Bar3D:
                                        eViewType = ViewType.Bar3D;
                                        break;
                                }
                                pSerieGraph.ChangeView(eViewType);
                            }
                            pSerieGraph.View.Color = pSerie.Color;
                            break;
                        }
                    }
                }
            }
        }
    }
}

