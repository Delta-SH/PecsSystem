<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Speaker.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.Speaker" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>告警语音播报</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <style type="text/css">
        .container {
            margin: 20px auto;
            vertical-align: middle;
            text-align: center;
        }
        .tips {
            font-size: 9pt;
            color: #FFFFFF;
            background: #666666;
            width: 600px;
            padding: 10px 0;
            margin:0 auto;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var player = null,
            speechs = null,
            textToSpeech = null,
            startTime = null,
            maxInterval = 30,
            PlayStates = {
                "Undefined": 0,
                "Stopped": 1,
                "Paused": 2,
                "Playing": 3,
                "ScanForward": 4,
                "ScanReverse": 5,
                "Buffering": 6,
                "Waiting": 7,
                "MediaEnded": 8,
                "Transitioning": 9,
                "Ready": 10,
                "Reconnecting": 11
            };

        var IEVersion = (function () {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf('MSIE ');
            var trident = ua.indexOf('Trident/');

            if (msie > 0) {
                return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
            }

            if (trident > 0) {
                var rv = ua.indexOf('rv:');
                return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
            }

            return -1;
        } ());

        var isIE = IEVersion > -1;

        var checkForWMP = function() {
            var activex = null;
            var plugin = null;
            
            try {
                if (window.ActiveXObject) {
                    activex = new ActiveXObject("WMPlayer.OCX.7");
                } else if (window.GeckoActiveXObject) {
                    activex = new GeckoActiveXObject("WMPlayer.OCX.7");
                }
            } catch (oError) { }
            
            try {
                if (navigator.mimeTypes) {
                    plugin = navigator.mimeTypes['application/x-mplayer2'].enabledPlugin;
                }
            } catch (oError) { }

            if (activex || plugin)
                return true;
            else
                return false;
        };
        var OnPlayHandler = function() {
            if (speechs && speechs.length > 0) { return; }
            X.Speaker.PlayHandler({
                success: function(result) {
                    if (!Ext.isEmpty(result, false)) {
                        speechs = Ext.util.JSON.decode(result, true);
                    }
                }
            });
        };
        var OnPlay = function() {
            if (speechs && speechs.length > 0) {
                if (!startTime) {
                    startTime = new Date().getTime();
                    textToSpeech = speechs.shift();
                    window.setTimeout("Play();", 1000);
                }
                else if ((new Date().getTime() - startTime) > maxInterval * 1000) {
                    Stop();
                    textToSpeech = null;
                    startTime = null;
                }
            }
        };
        var Play = function() {
            var url = "CreateWavStream.ashx?TextToSpeech=" + encodeURIComponent(textToSpeech);
            if (!player) { player = document.getElementById("MediaPlayer"); }
            if (player) {
                if (!isIE) {
                    player.src = url;
                    player.play();
                }
                else {
                    player.URL = url;
                    player.controls.play();
                }
            }
        };
        var Stop = function() {
            if (!player) { player = document.getElementById("MediaPlayer"); }
            if (player) {
                if (!isIE) {
                    player.pause();
                    player.currentTime = 0;
                }
                else {
                    player.controls.stop();
                }
            }
        }
        window.onload = function() {
            var container = document.getElementById("MediaContainer");
            if (container) {
                if (!isIE) {
                    if (window.HTMLAudioElement) {
                        var template = [
                        '<audio id="MediaPlayer" controls="controls" autoplay="true">',
                        '<source src="CreateWavStream.ashx" type="audio/wav">',
                        '<div class="tips">浏览器不支持HTML5 Audio，请将浏览器升级到最新版本。</div>',
                        '</audio>'];
                        container.innerHTML = template.join("");
                        player = document.getElementById("MediaPlayer");
                        if (player) {
                            if (window.addEventListener) {
                                player.addEventListener("ended", function() {
                                    textToSpeech = null;
                                    startTime = null;
                                }, false);
                            } else if (window.attachEvent) {
                                player.attachEvent("onended", function() {
                                    textToSpeech = null;
                                    startTime = null;
                                });
                            }
                        }
                    }
                } else {
                    if (checkForWMP()) {
                        var template = [
                        '<object id="MediaPlayer" classid="CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6" type="application/x-oleobject" width="450px" height="64px">',
                        '<param name="URL" value="CreateWavStream.ashx"/>',
                        '<param name="volume" value="100"/>',
                        '</object>'];
                        container.innerHTML = template.join("");
                        player = document.getElementById("MediaPlayer");
                    } else {
                        container.innerHTML = '<div class="tips">未安装Windows Media Player或安装版本较低，请安装最新版本的Windows Media Player。</div>';
                    }
                }
            }
        };
    </script>
    <script type="text/javascript" for="MediaPlayer" event="PlayStateChange(newState)">
        if (PlayStates.Stopped == newState) {
            textToSpeech = null;
            startTime = null;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <div class="container">
        <div id="MediaContainer">
        </div>
    </div>
    <ext:TaskManager ID="SpeakerTaskManager" runat="server">
        <Tasks>
            <ext:Task AutoRun="true" Interval="10000">
                <Listeners>
                    <Update Handler="OnPlayHandler();" />
                </Listeners>
            </ext:Task>
            <ext:Task AutoRun="true" Interval="5000">
                <Listeners>
                    <Update Handler="OnPlay();" />
                </Listeners>
            </ext:Task>
        </Tasks>
    </ext:TaskManager>
    </form>
</body>
</html>
