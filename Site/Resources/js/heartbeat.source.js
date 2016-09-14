/*
* HeartBeats JavaScript Library v3.0.0
*
* Copyright 2013-2014, Steven
*
* Date: 2014/08/28
*/

Ext.onReady(function () {
    Ext.TaskMgr.start({
        interval: 60000,
        run: function () {
            Ext.Ajax.request({ url: "/KeepAlive/Ping.ashx", success: function (response) { } });
        }
    });
});