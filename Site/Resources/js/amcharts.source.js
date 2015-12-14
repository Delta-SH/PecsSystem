/*
* AmCharts JavaScript Library v3.0.0
*
* Copyright 2013-2014, Steven
*
* Date: 2014/08/28
*/
Ext.ns("X");
Ext.data.Connection.override({
    timeout: 300000
});
Ext.Ajax.timeout = 300000;
Ext.net.DirectEvent.timeout = 300000;
var pageSize = { width: parseInt(Ext.getBody().getViewSize().width, 10), height: parseInt(Ext.getBody().getViewSize().height, 10) };
var RTCurve = {
    chart: null,
    dataProvider: null,
    container: null,
    chartGraph: null,
    render: function() {
        this.dataProvider = [];
        this.container = document.getElementById("chartContainer");
        if (!this.chart) { this.renderChart(); }
    },
    renderChart: function() {
        this.chart = new AmCharts.AmSerialChart();
        this.chart.pathToImages = "/Resources/amcharts/images/";
        this.chart.zoomOutButton = {
            backgroundColor: "#000000",
            backgroundAlpha: 0.15
        };
        this.chart.dataProvider = this.dataProvider;
        this.chart.categoryField = "amxName";
        this.chart.startDuration = 0;

        var categoryAxis = this.chart.categoryAxis;
        categoryAxis.parseDates = true;
        categoryAxis.dateFormats = [{ period: 'fff', format: 'JJ:NN:SS' }, { period: 'ss', format: 'JJ:NN:SS' }, { period: 'mm', format: 'JJ:NN' }, { period: 'hh', format: 'JJ:NN' }, { period: 'DD', format: 'MM/DD' }, { period: 'MM', format: 'YYYY/MM' }, { period: 'YYYY', format: 'YYYY'}];
        categoryAxis.minPeriod = "ss";
        categoryAxis.dashLength = 1;

        var valueAxis = new AmCharts.ValueAxis();
        valueAxis.dashLength = 1;
        valueAxis.fillColor = "#000000";
        valueAxis.fillAlpha = 0.05;
        this.chart.addValueAxis(valueAxis);

        var graph = new AmCharts.AmGraph();
        graph.title = "step line";
        graph.type = "step";
        graph.valueField = "amxValue";
        graph.descriptionField = "amxUnit";
        graph.bullet = "round";
        graph.balloonText = "[[description]]";
        graph.lineColor = "#0352B5";
        graph.negativeLineColor = "#00CC00";
        graph.hideBulletsCount = 100;
        this.chartGraph = graph;
        this.chart.addGraph(graph);

        var chartCursor = new AmCharts.ChartCursor();
        chartCursor.cursorAlpha = 1;
        chartCursor.cursorColor = "#0352B5";
        chartCursor.cursorPosition = "mouse";
        chartCursor.categoryBalloonDateFormat = "JJ:NN:SS";
        chartCursor.pan = true;
        this.chart.addChartCursor(chartCursor);

        var chartScrollbar = new AmCharts.ChartScrollbar();
        chartScrollbar.graph = graph;
        chartScrollbar.graphLineAlpha = 0.5;
        chartScrollbar.graphFillAlpha = 0;
        chartScrollbar.selectedGraphFillAlpha = 0;
        chartScrollbar.selectedGraphLineAlpha = 0.5;
        chartScrollbar.scrollbarHeight = 25;
        this.chart.addChartScrollbar(chartScrollbar);

        this.container.style.width = (pageSize.width - 5) + "px";
        this.container.style.height = (pageSize.height - 80) + "px";
        this.chart.write(this.container.id);
    },
    updateTitle: function() {
        X.RealTimeCurve.GetTitle({
            success: function(result) {
                if (!Ext.isEmpty(result, false)) {
                    if (RTCurve.chart) {
                        RTCurve.chart.addTitle(result, 15);
                        btnStart.fireEvent('click');
                    }
                }
            }
        });
    },
    appendData: function() {
        X.RealTimeCurve.GetData({
            success: function(result) {
                if (!Ext.isEmpty(result, false)) {
                    var data = Ext.util.JSON.decode(result, true);
                    if (data != null) {
                        if (RTCurve.dataProvider.length >= 360) { RTCurve.dataProvider.shift(); }
                        RTCurve.dataProvider.push({ amxName: new Date(Date.parse(data.amxName.replace(/-/g, "/"))), amxValue: data.amxValue, amxUnit: data.amxUnit });
                        RTCurve.chart.dataProvider = RTCurve.dataProvider;
                        RTCurve.chart.validateData();
                    }
                }
            }
        });
    }
};

var HisRTCurve = {
    chart: null,
    dataProvider: null,
    container: null,
    render: function() {
        this.initDataProvider();
        if (!this.chart) { this.renderChart(); }
    },
    initDataProvider: function() {
        this.dataProvider = [{ amxName: new Date(), amxValue: 0, amxDesc: null}];
    },
    renderChart: function() {
        this.chart = new AmCharts.AmSerialChart();
        this.chart.pathToImages = "/Resources/amcharts/images/";
        this.chart.zoomOutButton = {
            backgroundColor: "#000000",
            backgroundAlpha: 0.15
        };
        this.chart.dataProvider = this.dataProvider;
        this.chart.categoryField = "amxName";
        this.chart.addListener("dataUpdated", this.zoomChart);

        var categoryAxis = this.chart.categoryAxis;
        categoryAxis.parseDates = true;
        categoryAxis.dateFormats = [{ period: 'fff', format: 'YYYY/MM/DD JJ:NN:SS' }, { period: 'ss', format: 'JJ:NN:SS' }, { period: 'mm', format: 'JJ:NN' }, { period: 'hh', format: 'JJ:NN' }, { period: 'DD', format: 'YYYY/MM/DD' }, { period: 'MM', format: 'YYYY/MM' }, { period: 'YYYY', format: 'YYYY'}];
        categoryAxis.minPeriod = "fff";
        categoryAxis.dashLength = 1;

        var valueAxis = new AmCharts.ValueAxis();
        valueAxis.dashLength = 1;
        valueAxis.fillColor = "#000000";
        valueAxis.fillAlpha = 0.05;
        this.chart.addValueAxis(valueAxis);

        var graph = new AmCharts.AmGraph();
        graph.title = "detail line";
        graph.valueField = "amxValue";
        graph.descriptionField = "amxDesc";
        graph.bullet = "round";
        graph.bulletSize = 5;
        graph.bulletColor = "#FFFFFF";
        graph.bulletBorderColor = "#0352B5";
        graph.bulletBorderThickness = 1;
        graph.balloonText = "[[description]]";
        graph.lineColor = "#0352B5";
        graph.negativeLineColor = "#00CC00";
        graph.hideBulletsCount = 50;
        this.chart.addGraph(graph);

        var chartCursor = new AmCharts.ChartCursor();
        chartCursor.cursorAlpha = 1;
        chartCursor.cursorColor = "#0352B5";
        chartCursor.cursorPosition = "mouse";
        chartCursor.categoryBalloonDateFormat = "YYYY/MM/DD JJ:NN:SS";
        chartCursor.pan = true;
        this.chart.addChartCursor(chartCursor);

        var chartScrollbar = new AmCharts.ChartScrollbar();
        chartScrollbar.graph = graph;
        chartScrollbar.scrollbarHeight = 30;
        chartScrollbar.color = "#FFFFFF";
        chartScrollbar.autoGridCount = true;
        this.chart.addChartScrollbar(chartScrollbar);

        this.container = document.getElementById("chartContainer");
        this.container.style.width = (pageSize.width - 5) + "px";
        this.container.style.height = (pageSize.height * 0.6) + "px";
        this.chart.write(this.container.id);
    },
    zoomChart: function() {
        HisRTCurve.chart.zoomToIndexes(0, 200);
    },
    addTitle: function(title, size) {
        if (this.chart) { this.chart.addTitle(title, size); }
    },
    setData: function() {
        X.HisRealTimeCurve.GetData({
            success: function(result) {
                HisRTCurve.dataProvider = [];
                HisRTCurve.chart.titles = [];
                if (!Ext.isEmpty(result, false)) {
                    var datas = Ext.util.JSON.decode(result, true);
                    if (datas != null) {
                        Ext.each(datas.Data, function(data) {
                            HisRTCurve.dataProvider.push({ amxName: new Date(Date.parse(data.amxName.replace(/-/g, "/"))), amxValue: data.amxValue, amxDesc: data.amxDesc });
                        });

                        HisRTCurve.addTitle(datas.Title, 15);
                        HisRTCurve.addTitle(datas.SubTitle, 10);
                    }
                }

                if (HisRTCurve.dataProvider.length == 0) { HisRTCurve.initDataProvider(); }
                HisRTCurve.chart.dataProvider = HisRTCurve.dataProvider;
                HisRTCurve.chart.validateData();
            }
        });
    },
    setCenterPanel: function(el) {
        el.setHeight(pageSize.height * 0.7);
    },
    setSouthPanel: function(el) {
        el.setHeight(pageSize.height * 0.25);
    },
    getColumnNames: function(grid) {
        var xml = "<Names>";
        if (grid) {
            var columns = grid.getColumnModel().config;
            Ext.each(columns, function(c) {
                if (!c.hidden && !Ext.isEmpty(c.dataIndex, false))
                    xml += '<Name DataIndex="' + c.dataIndex + '" Header="' + c.header + '" />';
            });
        }

        xml += "</Names>";
        return xml;
    }
};

var HisCntCurve = {
    chart: null,
    dataProvider: null,
    container: null,
    render: function() {
        this.initDataProvider();
        if (!this.chart) { this.renderChart(); }
    },
    initDataProvider: function() {
        this.dataProvider = [{ amxName: new Date(), amxMaxValue: 0, amxMaxDesc: null}];
    },
    renderChart: function() {
        this.chart = new AmCharts.AmSerialChart();
        this.chart.pathToImages = "/Resources/amcharts/images/";
        this.chart.zoomOutButton = {
            backgroundColor: "#000000",
            backgroundAlpha: 0.15
        };
        this.chart.dataProvider = this.dataProvider;
        this.chart.categoryField = "amxName";
        this.chart.addListener("dataUpdated", this.zoomChart);

        var categoryAxis = this.chart.categoryAxis;
        categoryAxis.parseDates = true;
        categoryAxis.dateFormats = [{ period: 'fff', format: 'YYYY/MM/DD JJ:NN:SS' }, { period: 'ss', format: 'JJ:NN:SS' }, { period: 'mm', format: 'JJ:NN' }, { period: 'hh', format: 'JJ:NN' }, { period: 'DD', format: 'YYYY/MM/DD' }, { period: 'MM', format: 'YYYY/MM' }, { period: 'YYYY', format: 'YYYY'}];
        categoryAxis.minPeriod = "fff";
        categoryAxis.dashLength = 1;

        var valueAxis = new AmCharts.ValueAxis();
        valueAxis.dashLength = 1;
        valueAxis.fillColor = "#000000";
        valueAxis.fillAlpha = 0.05;
        this.chart.addValueAxis(valueAxis);

        var graph1 = new AmCharts.AmGraph();
        graph1.title = LanguageSet.MaxValue;
        graph1.type = "smoothedLine";
        graph1.valueField = "amxMaxValue";
        graph1.descriptionField = "amxMaxDesc";
        graph1.bullet = "round";
        graph1.bulletColor = "#FFFFFF";
        graph1.bulletBorderColor = "#0352B5";
        graph1.bulletBorderThickness = 1;
        graph1.bulletSize = 5;
        graph1.balloonText = "[[description]]";
        graph1.lineColor = "#0352B5";
        graph1.hideBulletsCount = 50;
        this.chart.addGraph(graph1);

        var graph2 = new AmCharts.AmGraph();
        graph2.title = LanguageSet.MinValue;
        graph2.type = "smoothedLine";
        graph2.valueField = "amxMinValue";
        graph2.descriptionField = "amxMinDesc";
        graph2.bullet = "round";
        graph2.bulletColor = "#FFFFFF";
        graph2.bulletBorderColor = "#00CC00";
        graph2.bulletBorderThickness = 1;
        graph2.bulletSize = 5;
        graph2.balloonText = "[[description]]";
        graph2.lineColor = "#00CC00";
        graph2.hideBulletsCount = 50;
        this.chart.addGraph(graph2);

        var graph3 = new AmCharts.AmGraph();
        graph3.title = LanguageSet.AvgValue;
        graph3.type = "smoothedLine";
        graph3.valueField = "amxAvgValue";
        graph3.descriptionField = "amxAvgDesc";
        graph3.bullet = "round";
        graph3.bulletColor = "#FFFFFF";
        graph3.bulletBorderColor = "#FFCC00";
        graph3.bulletBorderThickness = 1;
        graph3.bulletSize = 5;
        graph3.balloonText = "[[description]]";
        graph3.lineColor = "#FFCC00";
        graph3.hideBulletsCount = 50;
        this.chart.addGraph(graph3);

        var chartCursor = new AmCharts.ChartCursor();
        chartCursor.cursorAlpha = 1;
        chartCursor.cursorColor = "#0352B5";
        chartCursor.cursorPosition = "mouse";
        chartCursor.categoryBalloonDateFormat = "YYYY/MM/DD JJ:NN:SS";
        chartCursor.pan = true;
        this.chart.addChartCursor(chartCursor);

        var chartScrollbar = new AmCharts.ChartScrollbar();
        chartScrollbar.graph = graph3;
        chartScrollbar.scrollbarHeight = 30;
        chartScrollbar.color = "#FFFFFF";
        chartScrollbar.autoGridCount = true;
        this.chart.addChartScrollbar(chartScrollbar);

        var legend = new AmCharts.AmLegend();
        this.chart.addLegend(legend);

        this.container = document.getElementById("chartContainer");
        this.container.style.width = (pageSize.width - 5) + "px";
        this.container.style.height = (pageSize.height * 0.6) + "px";
        this.chart.write(this.container.id);
    },
    zoomChart: function() {
        HisCntCurve.chart.zoomToIndexes(0, 200);
    },
    addTitle: function(title, size) {
        if (this.chart) { this.chart.addTitle(title, size); }
    },
    setData: function() {
        X.HisCountCurve.GetData({
            success: function(result) {
                HisCntCurve.dataProvider = [];
                HisCntCurve.chart.titles = [];
                if (!Ext.isEmpty(result, false)) {
                    var datas = Ext.util.JSON.decode(result, true);
                    if (datas != null) {
                        Ext.each(datas.Data, function(data) {
                            HisCntCurve.dataProvider.push({ amxName: new Date(Date.parse(data.amxName.replace(/-/g, "/"))), amxMaxValue: data.amxMaxValue, amxMaxDesc: data.amxMaxDesc, amxMinValue: data.amxMinValue, amxMinDesc: data.amxMinDesc, amxAvgValue: data.amxAvgValue, amxAvgDesc: data.amxAvgDesc });
                        });

                        HisCntCurve.addTitle(datas.Title, 15);
                        HisCntCurve.addTitle(datas.SubTitle, 10);
                    }
                }

                if (HisCntCurve.dataProvider.length == 0) { HisCntCurve.initDataProvider(); }
                HisCntCurve.chart.dataProvider = HisCntCurve.dataProvider;
                HisCntCurve.chart.validateData();
            }
        });
    },
    setCenterPanel: function(el) {
        el.setHeight(pageSize.height * 0.7);
    },
    setSouthPanel: function(el) {
        el.setHeight(pageSize.height * 0.25);
    },
    getColumnNames: function(grid) {
        var xml = "<Names>";
        if (grid) {
            var columns = grid.getColumnModel().config;
            Ext.each(columns, function(c) {
                if (!c.hidden && !Ext.isEmpty(c.dataIndex, false))
                    xml += '<Name DataIndex="' + c.dataIndex + '" Header="' + c.header + '" />';
            });
        }

        xml += "</Names>";
        return xml;
    }
};

var StaAvailability = {
    chart: null,
    dataProvider: [{ amxName: "", amxValue: 0, amxColor: null}],
    container: null,
    initDataProvider: function() {
        this.dataProvider = [{ amxName: "", amxValue: 0, amxColor: null}];
    },
    renderAmSerialChart: function() {
        this.chart = new AmCharts.AmSerialChart();
        this.chart.dataProvider = this.dataProvider;
        this.chart.categoryField = "amxName";
        this.chart.startDuration = 1;
        this.chart.depth3D = 20;
        this.chart.angle = 30;

        var categoryAxis = this.chart.categoryAxis;
        categoryAxis.labelRotation = 45;
        categoryAxis.dashLength = 1;
        categoryAxis.gridPosition = "start";

        var valueAxis = new AmCharts.ValueAxis();
        valueAxis.title = LanguageSet.Availability;
        valueAxis.dashLength = 1;
        valueAxis.fillColor = "#000000";
        valueAxis.fillAlpha = 0.05;
        this.chart.addValueAxis(valueAxis);

        var graph = new AmCharts.AmGraph();
        graph.title = "station availability graph";
        graph.type = "column";
        graph.valueField = "amxValue";
        graph.colorField = "amxColor";
        graph.balloonText = "[[category]]: [[value]]%";
        graph.lineAlpha = 0;
        graph.fillAlphas = 1;
        this.chart.addGraph(graph);

        this.container = document.getElementById("chartContainer");
        this.container.style.width = (pageSize.width - 5) + "px";
        this.container.style.height = (pageSize.height * 0.5) + "px";
        this.container.innerHTML = "";
        this.chart.write(this.container.id);
    },
    renderAmPieChart: function() {
        this.chart = new AmCharts.AmPieChart();
        this.chart.dataProvider = this.dataProvider;
        this.chart.titleField = "amxName";
        this.chart.valueField = "amxValue";
        this.chart.startDuration = 1;
        this.chart.outlineColor = "#FFFFFF";
        this.chart.outlineAlpha = 0.8;
        this.chart.outlineThickness = 2;
        this.chart.depth3D = 15;
        this.chart.angle = 30;

        this.container = document.getElementById("chartContainer");
        this.container.style.width = (pageSize.width - 5) + "px";
        this.container.style.height = (pageSize.height * 0.5) + "px";
        this.container.innerHTML = "";
        this.chart.write(this.container.id);
    },
    addTitle: function(title, size) {
        if (this.chart) { this.chart.addTitle(title, size); }
    },
    setData: function() {
        X.StaAvailabilityReport.GetData({
            success: function(result) {
                StaAvailability.dataProvider = [];
                StaAvailability.chart.titles = [];
                if (!Ext.isEmpty(result, false)) {
                    var datas = Ext.util.JSON.decode(result, true);
                    if (datas != null) {
                        Ext.each(datas.Data, function(data) {
                            StaAvailability.dataProvider.push({ amxName: data.amxName, amxValue: data.amxValue, amxColor: data.amxColor });
                        });

                        //StaAvailability.addTitle(datas.Title, 15);
                    }
                }

                if (StaAvailability.dataProvider.length == 0) { StaAvailability.initDataProvider(); }
                StaAvailability.chart.dataProvider = StaAvailability.dataProvider;
                StaAvailability.chart.validateData();
            }
        });
    },
    setCenterPanel: function(el) {
        el.setHeight(pageSize.height * 0.6);
    },
    setSouthPanel: function(el) {
        el.setHeight(pageSize.height * 0.35);
    },
    getColumnNames: function(grid) {
        var xml = "<Names>";
        if (grid) {
            var columns = grid.getColumnModel().config;
            Ext.each(columns, function(c) {
                if (!c.hidden && !Ext.isEmpty(c.dataIndex, false))
                    xml += '<Name DataIndex="' + c.dataIndex + '" Header="' + c.header + '" />';
            });
        }

        xml += "</Names>";
        return xml;
    },
    selectType: function(e, rec) {
        switch (rec.data.value) {
            case "column":
                this.renderAmSerialChart();
                break;
            case "pie":
                this.renderAmPieChart();
                break;
            default:
                break;
        }
    },
    getValues: function(tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function(node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function(tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function(node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function(value) {
        var tree = this.component;
        if (tree.rendered) {
            X.StaAvailabilityReport.InitRateCountTreeNodes({
                success: function(result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function(el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function(c) {
                c.cascade(function(n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(StaAvailability.getValues(this), StaAvailability.getTexts(this), false);
    }
};

var DevAvailability = {
    chart: null,
    dataProvider: [{ amxName: "", amxValue: 0, amxColor: null}],
    container: null,
    initDataProvider: function() {
        this.dataProvider = [{ amxName: "", amxValue: 0, amxColor: null}];
    },
    renderAmSerialChart: function() {
        this.chart = new AmCharts.AmSerialChart();
        this.chart.dataProvider = this.dataProvider;
        this.chart.categoryField = "amxName";
        this.chart.startDuration = 1;
        this.chart.depth3D = 20;
        this.chart.angle = 30;

        var categoryAxis = this.chart.categoryAxis;
        categoryAxis.labelRotation = 45;
        categoryAxis.dashLength = 1;
        categoryAxis.gridPosition = "start";

        var valueAxis = new AmCharts.ValueAxis();
        valueAxis.title = LanguageSet.Availability;
        valueAxis.dashLength = 1;
        valueAxis.fillColor = "#000000";
        valueAxis.fillAlpha = 0.05;
        this.chart.addValueAxis(valueAxis);

        var graph = new AmCharts.AmGraph();
        graph.title = "device availability graph";
        graph.type = "column";
        graph.valueField = "amxValue";
        graph.colorField = "amxColor";
        graph.balloonText = "[[category]]: [[value]]%";
        graph.lineAlpha = 0;
        graph.fillAlphas = 1;
        this.chart.addGraph(graph);

        this.container = document.getElementById("chartContainer");
        this.container.style.width = (pageSize.width - 5) + "px";
        this.container.style.height = (pageSize.height * 0.5) + "px";
        this.container.innerHTML = "";
        this.chart.write(this.container.id);
    },
    renderAmPieChart: function() {
        this.chart = new AmCharts.AmPieChart();
        this.chart.dataProvider = this.dataProvider;
        this.chart.titleField = "amxName";
        this.chart.valueField = "amxValue";
        this.chart.startDuration = 1;
        this.chart.outlineColor = "#FFFFFF";
        this.chart.outlineAlpha = 0.8;
        this.chart.outlineThickness = 2;
        this.chart.depth3D = 15;
        this.chart.angle = 30;

        this.container = document.getElementById("chartContainer");
        this.container.style.width = (pageSize.width - 5) + "px";
        this.container.style.height = (pageSize.height * 0.5) + "px";
        this.container.innerHTML = "";
        this.chart.write(this.container.id);
    },
    addTitle: function(title, size) {
        if (this.chart) { this.chart.addTitle(title, size); }
    },
    setData: function() {
        X.DevAvailabilityReport.GetData({
            success: function(result) {
                DevAvailability.dataProvider = [];
                DevAvailability.chart.titles = [];
                if (!Ext.isEmpty(result, false)) {
                    var datas = Ext.util.JSON.decode(result, true);
                    if (datas != null) {
                        Ext.each(datas.Data, function(data) {
                            DevAvailability.dataProvider.push({ amxName: data.amxName, amxValue: data.amxValue, amxColor: data.amxColor });
                        });

                        //DevAvailability.addTitle(datas.Title, 15);
                    }
                }

                if (DevAvailability.dataProvider.length == 0) { DevAvailability.initDataProvider(); }
                DevAvailability.chart.dataProvider = DevAvailability.dataProvider;
                DevAvailability.chart.validateData();
            }
        });
    },
    setCenterPanel: function(el) {
        el.setHeight(pageSize.height * 0.6);
    },
    setSouthPanel: function(el) {
        el.setHeight(pageSize.height * 0.35);
    },
    getColumnNames: function(grid) {
        var xml = "<Names>";
        if (grid) {
            var columns = grid.getColumnModel().config;
            Ext.each(columns, function(c) {
                if (!c.hidden && !Ext.isEmpty(c.dataIndex, false))
                    xml += '<Name DataIndex="' + c.dataIndex + '" Header="' + c.header + '" />';
            });
        }

        xml += "</Names>";
        return xml;
    },
    selectType: function(e, rec) {
        switch (rec.data.value) {
            case "column":
                this.renderAmSerialChart();
                break;
            case "pie":
                this.renderAmPieChart();
                break;
            default:
                break;
        }
    },
    getValues: function(tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function(node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function(tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function(node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function(value) {
        var tree = this.component;
        if (tree.rendered) {
            X.DevAvailabilityReport.InitRateCountTreeNodes({
                success: function(result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function(el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function(c) {
                c.cascade(function(n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(DevAvailability.getValues(this), DevAvailability.getTexts(this), false);
    }
};

var DevPRCurve = {
    chart: null,
    dataProvider: null,
    container: null,
    render: function () {
        this.initDataProvider();
        if (!this.chart) {
            this.renderChart(); 
        }
    },
    initDataProvider: function () {
        this.dataProvider = [{ amxName: new Date(), amxValue: 0, amxDesc: null}];
    },
    renderChart: function () {
        this.chart = new AmCharts.AmSerialChart();
        this.chart.pathToImages = "/Resources/amcharts/images/";
        this.chart.zoomOutButton = {
            backgroundColor: "#000000",
            backgroundAlpha: 0.15
        };
        this.chart.dataProvider = this.dataProvider;
        this.chart.categoryField = "amxName";
        this.chart.addListener("dataUpdated", this.zoomChart);

        var categoryAxis = this.chart.categoryAxis;
        categoryAxis.parseDates = true;
        categoryAxis.dateFormats = [{ period: 'fff', format: 'YYYY/MM/DD JJ:NN:SS' }, { period: 'ss', format: 'JJ:NN:SS' }, { period: 'mm', format: 'JJ:NN' }, { period: 'hh', format: 'JJ:NN' }, { period: 'DD', format: 'YYYY/MM/DD' }, { period: 'MM', format: 'YYYY/MM' }, { period: 'YYYY', format: 'YYYY'}];
        categoryAxis.minPeriod = "fff";
        categoryAxis.dashLength = 1;

        var valueAxis = new AmCharts.ValueAxis();
        valueAxis.dashLength = 1;
        valueAxis.fillColor = "#000000";
        valueAxis.fillAlpha = 0.05;
        this.chart.addValueAxis(valueAxis);

        var graph = new AmCharts.AmGraph();
        graph.title = "detail line";
        graph.valueField = "amxValue";
        graph.descriptionField = "amxDesc";
        graph.bullet = "round";
        graph.bulletSize = 5;
        graph.bulletColor = "#FFFFFF";
        graph.bulletBorderColor = "#0352B5";
        graph.bulletBorderThickness = 1;
        graph.balloonText = "[[description]]";
        graph.lineColor = "#0352B5";
        graph.negativeLineColor = "#00CC00";
        graph.hideBulletsCount = 50;
        this.chart.addGraph(graph);

        var chartCursor = new AmCharts.ChartCursor();
        chartCursor.cursorAlpha = 1;
        chartCursor.cursorColor = "#0352B5";
        chartCursor.cursorPosition = "mouse";
        chartCursor.categoryBalloonDateFormat = "YYYY/MM/DD JJ:NN:SS";
        chartCursor.pan = true;
        this.chart.addChartCursor(chartCursor);

        var chartScrollbar = new AmCharts.ChartScrollbar();
        chartScrollbar.graph = graph;
        chartScrollbar.scrollbarHeight = 30;
        chartScrollbar.color = "#FFFFFF";
        chartScrollbar.autoGridCount = true;
        this.chart.addChartScrollbar(chartScrollbar);

        this.container = document.getElementById("chartContainer");
        this.container.style.width = (pageSize.width - 5) + "px";
        this.container.style.height = (pageSize.height * 0.6) + "px";
        this.chart.write(this.container.id);
    },
    zoomChart: function () {
        DevPRCurve.chart.zoomToIndexes(0, 200);
    },
    addTitle: function (title, size) {
        if (this.chart) { this.chart.addTitle(title, size); }
    },
    setData: function () {
        X.DevPerformanceReport.GetData({
            success: function (result) {
                DevPRCurve.dataProvider = [];
                DevPRCurve.chart.titles = [];
                if (!Ext.isEmpty(result, false)) {
                    var datas = Ext.util.JSON.decode(result, true);
                    if (datas != null) {
                        Ext.each(datas.Data, function (data) {
                            DevPRCurve.dataProvider.push({ amxName: new Date(Date.parse(data.amxName.replace(/-/g, "/"))), amxValue: data.amxValue, amxDesc: data.amxDesc });
                        });

                        DevPRCurve.addTitle(datas.Title, 15);
                        DevPRCurve.addTitle(datas.SubTitle, 10);
                    }
                }

                if (DevPRCurve.dataProvider.length == 0) { DevPRCurve.initDataProvider(); }
                DevPRCurve.chart.dataProvider = DevPRCurve.dataProvider;
                DevPRCurve.chart.validateData();
            }
        });
    },
    setCenterPanel: function (el) {
        el.setHeight(pageSize.height * 0.7);
    },
    setSouthPanel: function (el) {
        el.setHeight(pageSize.height * 0.25);
    },
    getColumnNames: function (grid) {
        var xml = "<Names>";
        if (grid) {
            var columns = grid.getColumnModel().config;
            Ext.each(columns, function (c) {
                if (!c.hidden && !Ext.isEmpty(c.dataIndex, false))
                    xml += '<Name DataIndex="' + c.dataIndex + '" Header="' + c.header + '" />';
            });
        }

        xml += "</Names>";
        return xml;
    }
};