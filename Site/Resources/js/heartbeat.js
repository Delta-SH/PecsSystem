Ext.onReady(function(){Ext.TaskMgr.start({interval:30000,run:function(){Ext.Ajax.request({url:"/KeepAlive/Ping.ashx",success:function(a){}})}})});
