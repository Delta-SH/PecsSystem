/*
* Baidu Map JavaScript Library v3.0.0
*
* Copyright 2013-2014, Steven
*
* Date: 2014/08/28
*/
/****************************************************SearchInfoWindow.js****************************************************/
var BMapLib = window.BMapLib = BMapLib || {};
var BMAPLIB_TAB_SEARCH = 0, BMAPLIB_TAB_TO_HERE = 1, BMAPLIB_TAB_FROM_HERE = 2;
(function() {
    var T, baidu = T = baidu || { version: '1.5.0' };
    baidu.guid = '$BAIDU$';
    (function() {
        window[baidu.guid] = window[baidu.guid] || {};
        baidu.lang = baidu.lang || {};
        baidu.lang.isString = function(source) {
            return '[object String]' == Object.prototype.toString.call(source);
        };
        baidu.lang.Event = function(type, target) {
            this.type = type;
            this.returnValue = true;
            this.target = target || null;
            this.currentTarget = null;
        };
        baidu.object = baidu.object || {};
        baidu.extend =
        baidu.object.extend = function(target, source) {
            for (var p in source) {
                if (source.hasOwnProperty(p)) {
                    target[p] = source[p];
                }
            }
            return target;
        };
        baidu.event = baidu.event || {};
        baidu.event._listeners = baidu.event._listeners || [];
        baidu.dom = baidu.dom || {};
        baidu.dom._g = function(id) {
            if (baidu.lang.isString(id)) {
                return document.getElementById(id);
            }
            return id;
        };
        baidu._g = baidu.dom._g;
        baidu.event.on = function(element, type, listener) {
            type = type.replace(/^on/i, '');
            element = baidu.dom._g(element);
            var realListener = function(ev) {
                listener.call(element, ev);
            },
                lis = baidu.event._listeners,
                filter = baidu.event._eventFilter,
                afterFilter,
                realType = type;
            type = type.toLowerCase();
            if (filter && filter[type]) {
                afterFilter = filter[type](element, type, realListener);
                realType = afterFilter.type;
                realListener = afterFilter.listener;
            }
            if (element.addEventListener) {
                element.addEventListener(realType, realListener, false);
            } else if (element.attachEvent) {
                element.attachEvent('on' + realType, realListener);
            }
            lis[lis.length] = [element, type, listener, realListener, realType];
            return element;
        };
        baidu.on = baidu.event.on;
        baidu.event.un = function(element, type, listener) {
            element = baidu.dom._g(element);
            type = type.replace(/^on/i, '').toLowerCase();

            var lis = baidu.event._listeners,
                len = lis.length,
                isRemoveAll = !listener,
                item,
                realType, realListener;
            while (len--) {
                item = lis[len];

                if (item[1] === type
                    && item[0] === element
                    && (isRemoveAll || item[2] === listener)) {
                    realType = item[4];
                    realListener = item[3];
                    if (element.removeEventListener) {
                        element.removeEventListener(realType, realListener, false);
                    } else if (element.detachEvent) {
                        element.detachEvent('on' + realType, realListener);
                    }
                    lis.splice(len, 1);
                }
            }
            return element;
        };
        baidu.un = baidu.event.un;
        baidu.dom.g = function(id) {
            if ('string' == typeof id || id instanceof String) {
                return document.getElementById(id);
            } else if (id && id.nodeName && (id.nodeType == 1 || id.nodeType == 9)) {
                return id;
            }
            return null;
        };
        baidu.g = baidu.G = baidu.dom.g;
        baidu.string = baidu.string || {};
        baidu.browser = baidu.browser || {};
        baidu.browser.ie = baidu.ie = /msie (\d+\.\d+)/i.test(navigator.userAgent) ? (document.documentMode || +RegExp['\x241']) : undefined;
        baidu.dom._NAME_ATTRS = (function() {
            var result = {
                'cellpadding': 'cellPadding',
                'cellspacing': 'cellSpacing',
                'colspan': 'colSpan',
                'rowspan': 'rowSpan',
                'valign': 'vAlign',
                'usemap': 'useMap',
                'frameborder': 'frameBorder'
            };
            if (baidu.browser.ie < 8) {
                result['for'] = 'htmlFor';
                result['class'] = 'className';
            } else {
                result['htmlFor'] = 'for';
                result['className'] = 'class';
            }
            return result;
        })();
        baidu.dom.setAttr = function(element, key, value) {
            element = baidu.dom.g(element);
            if ('style' == key) {
                element.style.cssText = value;
            } else {
                key = baidu.dom._NAME_ATTRS[key] || key;
                element.setAttribute(key, value);
            }
            return element;
        };
        baidu.setAttr = baidu.dom.setAttr;
        baidu.dom.setAttrs = function(element, attributes) {
            element = baidu.dom.g(element);
            for (var key in attributes) {
                baidu.dom.setAttr(element, key, attributes[key]);
            }
            return element;
        };
        baidu.setAttrs = baidu.dom.setAttrs;
        baidu.dom.create = function(tagName, opt_attributes) {
            var el = document.createElement(tagName),
                attributes = opt_attributes || {};
            return baidu.dom.setAttrs(el, attributes);
        };

        T.undope = true;
    })();

    var SearchInfoWindow =
        BMapLib.SearchInfoWindow = function(map, content, opts) {
            this.guid = guid++;
            BMapLib.SearchInfoWindow.instance[this.guid] = this;
            this._isOpen = false;
            this._map = map;
            this._opts = opts = opts || {};
            this._content = content || "";
            this._opts.width = opts.width;
            this._opts.height = opts.height;
            this._opts._title = opts.title || "";
            this._opts.offset = opts.offset || new BMap.Size(0, 0);
            this._opts.enableAutoPan = opts.enableAutoPan === false ? false : true;
            this._opts._panel = opts.panel || null;
            this._opts._searchTypes = opts.searchTypes;
        }
    SearchInfoWindow.prototype = new BMap.Overlay();
    SearchInfoWindow.prototype.initialize = function(map) {
        this._closeOtherSearchInfo();
        var me = this;
        var div = this._createSearchTemplate();
        var floatPane = map.getPanes().floatPane;
        floatPane.style.width = "auto";
        floatPane.appendChild(div);
        this._initSearchTemplate();
        this._getSearchInfoWindowSize();
        this._boxWidth = parseInt(this.container.offsetWidth, 10);
        this._boxHeight = parseInt(this.container.offsetHeight, 10);
        baidu.event.on(div, "onmousedown", function(e) {
            me._stopBubble(e);
        });
        baidu.event.on(div, "onmouseover", function(e) {
            me._stopBubble(e);
        });
        baidu.event.on(div, "contextmenu", function(e) {
            me._stopBubble(e);
        });
        baidu.event.on(div, "click", function(e) {
            me._stopBubble(e);
        });
        baidu.event.on(div, "dblclick", function(e) {
            me._stopBubble(e);
        });
        return div;
    }
    SearchInfoWindow.prototype.draw = function() {
        this._isOpen && this._adjustPosition(this._point);
    }
    SearchInfoWindow.prototype.open = function(anchor) {
        this._map.closeInfoWindow();
        var me = this, poi;
        if (!this._isOpen) {
            this._map.addOverlay(this);
            this._isOpen = true;
            setTimeout(function() {
                me._dispatchEvent(me, "open", { "point": me._point });
            }, 10);
        }
        if (anchor instanceof BMap.Point) {
            poi = anchor;
            this._removeMarkerEvt();
            this._marker = null;
        } else if (anchor instanceof BMap.Marker) {
            if (this._marker) {
                this._removeMarkerEvt();
            }
            poi = anchor.getPosition();
            this._marker = anchor;
            !this._markerDragend && this._marker.addEventListener("dragend", this._markerDragend = function(e) {
                me._point = e.point;
                me._adjustPosition(me._point);
                me._panBox();
                me.show();
            });
            !this._markerDragging && this._marker.addEventListener("dragging", this._markerDragging = function() {
                me.hide();
                me._point = me._marker.getPosition();
                me._adjustPosition(me._point);
            });
        }
        this.show();
        this._point = poi;
        this._panBox();
        this._adjustPosition(this._point);
    }
    SearchInfoWindow.prototype.close = function() {
        if (this._isOpen) {
            this._map.removeOverlay(this);
            this._disposeAutoComplete();
            this._isOpen = false;
            this._dispatchEvent(this, "close", { "point": this._point });
        }
    }
    SearchInfoWindow.prototype.enableAutoPan = function() {
        this._opts.enableAutoPan = true;
    }
    SearchInfoWindow.prototype.disableAutoPan = function() {
        this._opts.enableAutoPan = false;
    }
    SearchInfoWindow.prototype.setContent = function(content) {
        this._setContent(content);
        this._getSearchInfoWindowSize();
        this._adjustPosition(this._point);
    },
    SearchInfoWindow.prototype.setTitle = function(title) {
        this.dom.title.innerHTML = title;
        this._opts._title = title;
    }
    SearchInfoWindow.prototype.getContent = function() {
        return this.dom.content.innerHTML;
    },
    SearchInfoWindow.prototype.getTitle = function() {
        return this.dom.title.innerHTML;
    }
    SearchInfoWindow.prototype.setPosition = function(poi) {
        this._point = poi;
        this._adjustPosition(poi);
        this._panBox();
        this._removeMarkerEvt();
    }
    SearchInfoWindow.prototype.getPosition = function() {
        return this._point;
    }
    SearchInfoWindow.prototype.getOffset = function() {
        return this._opts.offset;
    },
    baidu.object.extend(SearchInfoWindow.prototype, {
        _closeOtherSearchInfo: function() {
            var instance = BMapLib.SearchInfoWindow.instance,
                len = instance.length;
            while (len--) {
                if (instance[len]._isOpen) {
                    instance[len].close();
                }
            }
        },
        _setContent: function(content) {
            if (!this.dom || !this.dom.content) {
                return;
            }
            if (typeof content.nodeType === "undefined") {
                this.dom.content.innerHTML = content;
            } else {
                this.dom.content.appendChild(content);
            }
            var me = this;
            me._adjustContainerWidth();
            this._content = content;
        },
        _adjustPosition: function(poi) {
            var pixel = this._getPointPosition(poi);
            var icon = this._marker && this._marker.getIcon();
            if (this._marker) {
                this.container.style.bottom = -(pixel.y - this._opts.offset.height - icon.anchor.height + icon.infoWindowAnchor.height) - this._marker.getOffset().height + 2 + 30 + "px";
                this.container.style.left = pixel.x - icon.anchor.width + this._marker.getOffset().width + icon.infoWindowAnchor.width - this._boxWidth / 2 + 28 + "px";
            } else {
                this.container.style.bottom = -(pixel.y - this._opts.offset.height) + 30 + "px";
                this.container.style.left = pixel.x - this._boxWidth / 2 + 25 + "px";
            }
        },
        _getPointPosition: function(poi) {
            this._pointPosition = this._map.pointToOverlayPixel(poi);
            return this._pointPosition;
        },
        _getSearchInfoWindowSize: function() {
            this._boxWidth = parseInt(this.container.offsetWidth, 10);
            this._boxHeight = parseInt(this.container.offsetHeight, 10);
        },
        _stopBubble: function(e) {
            if (e && e.stopPropagation) {
                e.stopPropagation();
            } else {
                window.event.cancelBubble = true;
            }
        },
        _panBox: function() {
            if (!this._opts.enableAutoPan) {
                return;
            }
            var mapH = parseInt(this._map.getContainer().offsetHeight, 10),
                mapW = parseInt(this._map.getContainer().offsetWidth, 10),
                boxH = this._boxHeight,
                boxW = this._boxWidth;
            if (boxH >= mapH || boxW >= mapW) {
                return;
            }
            if (!this._map.getBounds().containsPoint(this._point)) {
                this._map.setCenter(this._point);
            }
            var anchorPos = this._map.pointToPixel(this._point),
                panTop, panY,
                panLeft = boxW / 2 - 28 - anchorPos.x + 10,
                panRight = boxW / 2 + 28 + anchorPos.x - mapW + 10;
            if (this._marker) {
                var icon = this._marker.getIcon();
            }

            var h = this._marker ? icon.anchor.height + this._marker.getOffset().height - icon.infoWindowAnchor.height : 0;
            panTop = boxH - anchorPos.y + this._opts.offset.height + h + 31 + 10;
            panX = panLeft > 0 ? panLeft : (panRight > 0 ? -panRight : 0);
            panY = panTop > 0 ? panTop : 0;
            this._map.panBy(panX, panY);
        },
        _removeMarkerEvt: function() {
            this._markerDragend && this._marker.removeEventListener("dragend", this._markerDragend);
            this._markerDragging && this._marker.removeEventListener("dragging", this._markerDragging);
            this._markerDragend = this._markerDragging = null;
        },
        _dispatchEvent: function(instance, type, opts) {
            type.indexOf("on") != 0 && (type = "on" + type);
            var event = new baidu.lang.Event(type);
            if (!!opts) {
                for (var p in opts) {
                    event[p] = opts[p];
                }
            }
            instance.dispatchEvent(event);
        },
        _initSearchTemplate: function() {
            this._initDom();
            this._initPanelTemplate();
            this.setTitle(this._opts._title);
            if (this._opts.height) {
                this.dom.content.style.height = parseInt(this._opts.height, 10) + "px";
            }
            this._setContent(this._content);
            this._initService();
            this._bind();
            if (this._opts._searchTypes) {
                this._setSearchTypes();
            }
            this._mendIE6();
        },
        _createSearchTemplate: function() {
            if (!this._div) {
                var div = baidu.dom.create('div', {
                    "class": "BMapLib_SearchInfoWindow",
                    "id": "BMapLib_SearchInfoWindow" + this.guid
                });
                var template = [
                    '<div class="BMapLib_bubble_top">',
                        '<div class="BMapLib_bubble_title" id="BMapLib_bubble_title' + this.guid + '"></div>',
                        '<div class="BMapLib_bubble_close" id="BMapLib_bubble_close' + this.guid + '">',
                            '<span>关闭</span>',
                        '</div>',
                    '</div>',
                    '<div class="BMapLib_bubble_center">',
                        '<div class="BMapLib_bubble_content" id="BMapLib_bubble_content' + this.guid + '">',
                        '</div>',
                        '<div class="BMapLib_nav" id="BMapLib_nav' + this.guid + '">',
                            '<ul class="BMapLib_nav_tab" id="BMapLib_nav_tab' + this.guid + '">',
                                '<li class="BMapLib_first BMapLib_current" id="BMapLib_tab_search' + this.guid + '" style="display:block;">',
                                    '<span class="BMapLib_icon BMapLib_icon_nbs"></span>在附近找',
                                '</li>',
                                '<li class="" id="BMapLib_tab_tohere' + this.guid + '" style="display:block;">',
                                    '<span class="BMapLib_icon BMapLib_icon_tohere"></span>到这里去',
                                '</li>',
                                '<li class="" id="BMapLib_tab_fromhere' + this.guid + '" style="display:block;">',
                                    '<span class="BMapLib_icon BMapLib_icon_fromhere"></span>从这里出发',
                                '</li>',
                            '</ul>',
                            '<ul class="BMapLib_nav_tab_content">',
                                '<li id="BMapLib_searchBox' + this.guid + '">',
                                    '<table width="100%" align="center" border=0 cellpadding=0 cellspacing=0>',
                                        '<tr><td style="padding-left:8px;"><input id="BMapLib_search_text' + this.guid + '" class="BMapLib_search_text" type="text" maxlength="100" autocomplete="off"></td><td width="55" style="padding-left:7px;"><input id="BMapLib_search_nb_btn' + this.guid + '" type="submit" value="搜索" class="iw_bt"></td></tr>',
                                    '</table>',
                                '</li>',
                                '<li id="BMapLib_transBox' + this.guid + '" style="display:none">',
                                    '<table width="100%" align="center" border=0 cellpadding=0 cellspacing=0>',
                                        '<tr><td width="30" style="padding-left:8px;"><div id="BMapLib_stationText' + this.guid + '">起点</div></td><td><input id="BMapLib_trans_text' + this.guid + '" class="BMapLib_trans_text" type="text" maxlength="100" autocomplete="off"></td><td width="106" style="padding-left:7px;"><input id="BMapLib_search_bus_btn' + this.guid + '" type="button" value="公交" class="iw_bt" style="margin-right:5px;"><input id="BMapLib_search_drive_btn' + this.guid + '" type="button" class="iw_bt" value="驾车"></td></tr>',
                                    '</table>',
                                '</li>',
                            '</ul>',
                        '</div>',
                    '</div>',
                    '<div class="BMapLib_bubble_bottom"></div>',
                    '<img src="Resources/images/Baidu/SearchInfoWindow/iw_tail.png" width="58" height="31" alt="" class="BMapLib_trans" id="BMapLib_trans' + this.guid + '" style="left:144px;"/>'
                ];
                div.innerHTML = template.join("");
                this._div = div;
            }
            return this._div;
        },
        _initPanelTemplate: function() {
            var panel = baidu.g(this._opts._panel);
            if (!this.dom.panel && panel) {
                panel.innerHTML = "";
                this.dom.panel = panel;
                var address = baidu.dom.create('div');
                address.style.cssText = "display:none;background:#FD9;height:30px;line-height:30px;text-align:center;font-size:12px;color:#994C00;";
                panel.appendChild(address);
                this.dom.panel.address = address;
                var localSearch = baidu.dom.create('div');
                panel.appendChild(localSearch);
                this.dom.panel.localSearch = localSearch;
            }
        },
        _initDom: function() {
            if (!this.dom) {
                this.dom = {
                    container: baidu.g("BMapLib_SearchInfoWindow" + this.guid) //容器
                    , content: baidu.g("BMapLib_bubble_content" + this.guid)   //主内容
                    , title: baidu.g("BMapLib_bubble_title" + this.guid)     //标题
                    , closeBtn: baidu.g("BMapLib_bubble_close" + this.guid)     //关闭按钮
                    , transIco: baidu.g("BMapLib_trans" + this.guid)            //infowindow底下三角形
                    , navBox: baidu.g("BMapLib_nav" + this.guid)              //检索容器
                    , navTab: baidu.g("BMapLib_nav_tab" + this.guid)          //tab容器
                    , seartTab: baidu.g("BMapLib_tab_search" + this.guid)       //在附近找tab
                    , tohereTab: baidu.g("BMapLib_tab_tohere" + this.guid)       //到这里去tab
                    , fromhereTab: baidu.g("BMapLib_tab_fromhere" + this.guid)     //从这里出发tab
                    , searchBox: baidu.g("BMapLib_searchBox" + this.guid)        //普通检索容器
                    , transBox: baidu.g("BMapLib_transBox" + this.guid)         //公交驾车检索容器，从这里出发和到这里去公用一个容器
                    , stationText: baidu.g("BMapLib_stationText" + this.guid)      //起点或终点文本
                    , nbBtn: baidu.g("BMapLib_search_nb_btn" + this.guid)    //普通检索按钮
                    , busBtn: baidu.g("BMapLib_search_bus_btn" + this.guid)   //公交驾车检索按钮
                    , driveBtn: baidu.g("BMapLib_search_drive_btn" + this.guid) //驾车检索按钮
                    , searchText: baidu.g("BMapLib_search_text" + this.guid)      //普通检索文本框
                    , transText: baidu.g("BMapLib_trans_text" + this.guid)       //公交驾车检索文本框
                }
                this.container = this.dom.container;
            }
        },
        _adjustContainerWidth: function() {
            var width = 250,
                height = 0;
            if (this._opts.width) {
                width = parseInt(this._opts.width, 10);
                width += 10;
            } else {
                width = parseInt(this.dom.content.offsetWidth, 10);
            }
            if (width < 250) {
                width = 250;
            }
            this._width = width;
            this.container.style.width = this._width + "px";
            this._adjustTransPosition();
        },
        _adjustTransPosition: function() {
            this.dom.transIco.style.left = this.container.offsetWidth / 2 - 2 - 29 + "px";
            this.dom.transIco.style.top = this.container.offsetHeight - 2 + "px";
        },
        _initService: function() {
            var map = this._map;
            var me = this;
            var renderOptions = {}
            renderOptions.map = map;
            if (this.dom.panel) {
                renderOptions.panel = this.dom.panel.localSearch;
            }
            if (!this.localSearch) {
                this.localSearch = new BMap.LocalSearch(map, {
                    renderOptions: renderOptions
                    , onSearchComplete: function(result) {
                        me._clearAddress();
                        me._drawCircleBound();
                    }
                });
            }
            if (!this.transitRoute) {
                this.transitRoute = new BMap.TransitRoute(map, {
                    renderOptions: renderOptions
                    , onSearchComplete: function(results) {
                        me._transitRouteComplete(me.transitRoute, results);
                    }
                });
            }
            if (!this.drivingRoute) {
                this.drivingRoute = new BMap.DrivingRoute(map, {
                    renderOptions: renderOptions
                    , onSearchComplete: function(results) {
                        me._transitRouteComplete(me.drivingRoute, results);
                    }
                });
            }
        },
        _bind: function() {
            var me = this;
            baidu.on(this.dom.closeBtn, "click", function(e) {
                me.close();
            });
            baidu.on(this.dom.seartTab, "click", function(e) {
                me._showTabContent(BMAPLIB_TAB_SEARCH);
            });
            baidu.on(this.dom.tohereTab, "click", function(e) {
                me._showTabContent(BMAPLIB_TAB_TO_HERE);
            });
            baidu.on(this.dom.fromhereTab, "click", function(e) {
                me._showTabContent(BMAPLIB_TAB_FROM_HERE);
            });
            baidu.on(this.dom.nbBtn, "click", function(e) {
                me._localSearchAction();
            });
            baidu.on(this.dom.busBtn, "click", function(e) {
                me._transitRouteAction(me.transitRoute);
            });
            baidu.on(this.dom.driveBtn, "click", function(e) {
                me._transitRouteAction(me.drivingRoute);
            });
            this._autoCompleteIni();
        },
        _showTabContent: function(type) {
            this._hideAutoComplete();
            var tabs = this.dom.navTab.getElementsByTagName("li"),
                len = tabs.length;
            for (var i = 0, len = tabs.length; i < len; i++) {
                tabs[i].className = "";
            }
            switch (type) {
                case BMAPLIB_TAB_SEARCH:
                    this.dom.seartTab.className = "BMapLib_current";
                    this.dom.searchBox.style.display = "block";
                    this.dom.transBox.style.display = "none";
                    break;
                case BMAPLIB_TAB_TO_HERE:
                    this.dom.tohereTab.className = "BMapLib_current";
                    this.dom.searchBox.style.display = "none";
                    this.dom.transBox.style.display = "block";
                    this.dom.stationText.innerHTML = "起点";
                    this._pointType = "endPoint";
                    break;
                case BMAPLIB_TAB_FROM_HERE:
                    this.dom.fromhereTab.className = "BMapLib_current";
                    this.dom.searchBox.style.display = "none";
                    this.dom.transBox.style.display = "block";
                    this.dom.stationText.innerHTML = "终点";
                    this._pointType = "startPoint";
                    break;
            }
            this._firstTab.className += " BMapLib_first";
        },
        _autoCompleteIni: function() {
            this.searchAC = new BMap.Autocomplete({
                "input": this.dom.searchText
                , "location": this._map
            });
            this.transAC = new BMap.Autocomplete({
                "input": this.dom.transText
                , "location": this._map
            });
        },
        _hideAutoComplete: function() {
            this.searchAC.hide();
            this.transAC.hide();
        },
        _disposeAutoComplete: function() {
            this.searchAC.dispose();
            this.transAC.dispose();
        },
        _localSearchAction: function() {
            var kw = this._kw = this.dom.searchText.value;
            if (kw == "") {
                this.dom.searchText.focus();
            } else {
                this._reset();
                this.close();
                var radius = this._radius = 1000;
                this.localSearch.searchNearby(kw, this._point, radius);
            }
        },
        _drawCircleBound: function() {
            this._closeCircleBound();
            var circle = this._searchCircle = new BMap.Circle(this._point, this._radius, {
                strokeWeight: 3,
                strokeOpacity: 0.4,
                strokeColor: "#e00",
                filColor: "#00e",
                fillOpacity: 0.4
            });
            var label = this._searchLabel = new BMap.Label('<div onmousedown ="BMapLib.SearchInfoWindow.instance[' + this.guid + ']._stopBubble()"><input type="text" value="' + this._radius + '" style="width:30px;" id="BMapLib_search_radius' + this.guid + '"/>m <a href="javascript:void(0)" title="修改" onclick="BMapLib.SearchInfoWindow.instance[' + this.guid + ']._changeSearchRadius()" style="text-decoration:none;color:blue;">修改</a><img src="Resources/images/Baidu/SearchInfoWindow/iw_close.gif" alt="关闭" title="关闭" style="cursor:pointer;padding:0 5px;" onclick="BMapLib.SearchInfoWindow.instance[' + this.guid + ']._closeCircleBound()"/></div>', {
                position: this._point
            });
            this._map.addOverlay(circle);
            this._map.addOverlay(label);
            this._hasCircle = true;
        },
        _changeSearchRadius: function() {
            var radius = parseInt(baidu.g("BMapLib_search_radius" + this.guid).value, 10);
            if (radius > 0 && radius != this._radius) {
                this._radius = radius;
                this.localSearch.searchNearby(this._kw, this._point, radius);
                this._closeCircleBound();
            }
        },
        _closeCircleBound: function(radius) {
            if (this._searchCircle) {
                this._map.removeOverlay(this._searchCircle);
            }
            if (this._searchLabel) {
                this._map.removeOverlay(this._searchLabel);
            }
            this._hasCircle = false;
        },
        _transitRouteAction: function(transitDrive) {
            var kw = this.dom.transText.value;
            if (kw == "") {
                this.dom.transText.focus();
            } else {
                this._reset();
                this.close();
                var transPoi = this._getTransPoi(kw);
                transitDrive.search(transPoi.start, transPoi.end);
            }
        },
        _transitRouteComplete: function(transitDrive, results) {
            this._clearAddress();
            var status = transitDrive.getStatus();
            if (status == BMAP_STATUS_UNKNOWN_ROUTE) {
                var startStatus = results.getStartStatus(),
                    endStatus = results.getEndStatus(),
                    tip = "";
                tip = "找不到相关的线路";
                if (startStatus == BMAP_ROUTE_STATUS_EMPTY && endStatus == BMAP_ROUTE_STATUS_EMPTY) {
                    tip = "找不到相关的起点和终点";
                } else {
                    if (startStatus == BMAP_ROUTE_STATUS_EMPTY) {
                        tip = "找不到相关的起点";
                    }
                    if (endStatus == BMAP_ROUTE_STATUS_EMPTY) {
                        tip = "找不到相关的终点";
                    }
                }
                if (this._pointType == "startPoint" && endStatus == BMAP_ROUTE_STATUS_ADDRESS || this._pointType == "endPoint" && startStatus == BMAP_ROUTE_STATUS_ADDRESS) {
                    this._searchAddress(transitDrive);
                } else {
                    this.dom.panel.address.style.display = "block";
                    this.dom.panel.address.innerHTML = tip;
                }
            }
        },
        _searchAddress: function(transitDrive) {
            var me = this;
            var panel = this.dom.panel;
            if (!this.lsAddress) {
                var renderOptions = { map: this._map };
                if (panel) {
                    renderOptions.panel = this.dom.panel.localSearch;
                }
                this.lsAddress = new BMap.LocalSearch(map, { renderOptions: renderOptions });
            }
            var station = me._pointType == "startPoint" ? "终点" : "起点";
            if (panel) {
                this.dom.panel.address.style.display = "block";
                this.dom.panel.address.innerHTML = "请选择准确的" + station;
            }
            this.lsAddress.setInfoHtmlSetCallback(function(poi, html) {
                var button = document.createElement('div');
                button.style.cssText = "position:relative;left:50%;margin:5px 0 0 -30px;width:60px;height:27px;line-height:27px;border:1px solid #E0C3A6;text-align:center;color:#B35900;cursor:pointer;background-color:#FFEECC;border-radius:2px; background-image: -webkit-gradient(linear, left top, left bottom, from(#FFFDF8), to(#FFEECC))";
                button.innerHTML = '设为' + station;
                html.appendChild(button);
                baidu.on(button, "click", function() {
                    me._clearAddress();
                    var nowPoint = poi.marker.getPosition();
                    if (station == "起点") {
                        transitDrive.search(nowPoint, me._point);
                    } else {
                        transitDrive.search(me._point, nowPoint);
                    }
                });
            });
            this._reset();
            this.lsAddress.search(this.dom.transText.value);
        },
        _getTransPoi: function(kw) {
            var start, end;
            if (this._pointType == "startPoint") {
                start = this._point;
                end = kw;
            } else {
                start = kw;
                end = this._point;
            }
            return {
                "start": start,
                "end": end
            }
        },
        _setSearchTypes: function() {
            var searchTypes = this._unique(this._opts._searchTypes),
                navTab = this.dom.navTab,
                tabs = [this.dom.seartTab, this.dom.tohereTab, this.dom.fromhereTab],
                i = 0,
                len = 0,
                curIndex = 0,
                tab;
            this.tabLength = searchTypes.length;
            tabWidth = Math.floor((this._width - this.tabLength + 1) / this.tabLength);
            if (searchTypes.length == 0) {
                this.dom.navBox.style.display = "none";
            } else {
                for (i = 0, len = tabs.length; i < len; i++) {
                    tabs[i].className = "";
                    tabs[i].style.display = "none";
                }
                for (i = 0; i < this.tabLength; i++) {
                    tab = tabs[searchTypes[i]];
                    if (i == 0) {
                        tab.className = "BMapLib_first BMapLib_current";
                        this._firstTab = tab;
                        curIndex = searchTypes[i];
                    }
                    if (i == this.tabLength - 1) {
                        var lastWidth = this._width - (this.tabLength - 1) * (tabWidth + 1);
                        if (baidu.browser.ie == 6) {
                            tab.style.width = lastWidth - 3 + "px";
                        } else {
                            tab.style.width = lastWidth + "px";
                        }
                    } else {
                        tab.style.width = tabWidth + "px";
                    }
                    tab.style.display = "block";
                }
                if (searchTypes[1] != undefined) {
                    navTab.appendChild(tabs[searchTypes[1]])
                }
                if (searchTypes[2] != undefined) {
                    navTab.appendChild(tabs[searchTypes[2]])
                }
                this._showTabContent(curIndex);
            }
            this._adjustTransPosition();
        },
        _unique: function(arr) {
            var len = arr.length,
                result = arr.slice(0),
                i,
                datum;
            while (--len >= 0) {
                datum = result[len];
                if (datum < 0 || datum > 2) {
                    result.splice(len, 1);
                    continue;
                }
                i = len;
                while (i--) {
                    if (datum == result[i]) {
                        result.splice(len, 1);
                        break;
                    }
                }
            }
            return result;
        },
        _reset: function() {
            this.localSearch.clearResults();
            this.transitRoute.clearResults();
            this.drivingRoute.clearResults();
            this._closeCircleBound();
            this._hideAutoComplete();
        },
        _clearAddress: function() {
            if (this.lsAddress) {
                this.lsAddress.clearResults();
            }
            if (this.dom.panel) {
                this.dom.panel.address.style.display = "none";
            }
        },
        _mendIE6: function(infoWin) {
            if (!baidu.browser.ie || baidu.browser.ie > 6) {
                return;
            }
            var popImg = this.container.getElementsByTagName("IMG");
            for (var i = 0; i < popImg.length; i++) {
                if (popImg[i].src.indexOf('.png') < 0) {
                    continue;
                }
                popImg[i].style.cssText += ';FILTER: progid:DXImageTransform.Microsoft.AlphaImageLoader(src=' + popImg[i].src + ',sizingMethod=crop)'
                popImg[i].src = "Resources/images/Baidu/SearchInfoWindow/blank.gif";
            }
        }
    });
    var guid = 0;
    BMapLib.SearchInfoWindow.instance = [];
})();
/****************************************************End****************************************************/

/****************************************************Convertor.js****************************************************/
(function() {
    var translate = function(points, type, callback) {
        if (!points || points.length == 0) {
            callback && callback(null);
            return false;
        }

        var xs = [];
        var ys = [];
        var maxCnt = 20;
        for (var index = 0; index < points.length; index++) {
            if (index >= maxCnt) { break; }
            xs.push(points[index].lng);
            ys.push(points[index].lat);
        }
        send(xs, ys, type, callback);
    };
    var send = function(xs, ys, type, callback) {
        var callbackName = 'cbk_' + Math.round(Math.random() * 10000);
        BMap.Convertor[callbackName] = function(xyResults) {
            delete BMap.Convertor[callbackName];
            callback && callback(xyResults);
        };

        var xyUrl = "http://api.map.baidu.com/ag/coord/convert?from=" + (type || 0) + "&to=4&mode=1&x=" + xs.join(",") + "&y=" + ys.join(",") + "&callback=BMap.Convertor." + callbackName;
        loadScript(xyUrl);
        xs = [];
        ys = [];
    };
    var loadScript = function(xyUrl, callback) {
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = xyUrl;
        script.onload = script.onreadystatechange = function() {
            if ((!this.readyState || this.readyState === "loaded" || this.readyState === "complete")) {
                callback && callback();
                script.onload = script.onreadystatechange = null;
                if (head && script.parentNode) { head.removeChild(script); }
            }
        };
        head.insertBefore(script, head.firstChild);
    };

    window.BMap = window.BMap || {};
    BMap.Convertor = {};
    BMap.Convertor.translate = translate;
})();
/****************************************************End****************************************************/

/****************************************************Map.js****************************************************/
Ext.ns("X");
var cityCenter = { municipalities: [{ n: "北京", g: "116.395645,39.929986|12" }, { n: "上海", g: "121.487899,31.249162|12" }, { n: "天津", g: "117.210813,39.14393|12" }, { n: "重庆", g: "106.530635,29.544606|12"}], provinces: [{ n: "安徽", g: "117.216005,31.859252|8", cities: [{ n: "合肥", g: "117.282699,31.866942|12" }, { n: "安庆", g: "117.058739,30.537898|13" }, { n: "蚌埠", g: "117.35708,32.929499|13" }, { n: "亳州", g: "115.787928,33.871211|13" }, { n: "巢湖", g: "117.88049,31.608733|13" }, { n: "池州", g: "117.494477,30.660019|14" }, { n: "滁州", g: "118.32457,32.317351|13" }, { n: "阜阳", g: "115.820932,32.901211|13" }, { n: "淮北", g: "116.791447,33.960023|13" }, { n: "淮南", g: "117.018639,32.642812|13" }, { n: "黄山", g: "118.29357,29.734435|13" }, { n: "六安", g: "116.505253,31.755558|13" }, { n: "马鞍山", g: "118.515882,31.688528|13" }, { n: "宿州", g: "116.988692,33.636772|13" }, { n: "铜陵", g: "117.819429,30.94093|14" }, { n: "芜湖", g: "118.384108,31.36602|12" }, { n: "宣城", g: "118.752096,30.951642|13"}] }, { n: "福建", g: "117.984943,26.050118|8", cities: [{ n: "福州", g: "119.330221,26.047125|12" }, { n: "龙岩", g: "117.017997,25.078685|13" }, { n: "南平", g: "118.181883,26.643626|13" }, { n: "宁德", g: "119.542082,26.656527|14" }, { n: "莆田", g: "119.077731,25.44845|13" }, { n: "泉州", g: "118.600362,24.901652|12" }, { n: "三明", g: "117.642194,26.270835|14" }, { n: "厦门", g: "118.103886,24.489231|12" }, { n: "漳州", g: "117.676205,24.517065|12"}] }, { n: "甘肃", g: "102.457625,38.103267|6", cities: [{ n: "兰州", g: "103.823305,36.064226|12" }, { n: "白银", g: "104.171241,36.546682|13" }, { n: "定西", g: "104.626638,35.586056|13" }, { n: "甘南州", g: "102.917442,34.992211|14" }, { n: "嘉峪关", g: "98.281635,39.802397|13" }, { n: "金昌", g: "102.208126,38.516072|13" }, { n: "酒泉", g: "98.508415,39.741474|13" }, { n: "临夏州", g: "103.215249,35.598514|13" }, { n: "陇南", g: "104.934573,33.39448|14" }, { n: "平凉", g: "106.688911,35.55011|13" }, { n: "庆阳", g: "107.644227,35.726801|13" }, { n: "天水", g: "105.736932,34.584319|13" }, { n: "武威", g: "102.640147,37.933172|13" }, { n: "张掖", g: "100.459892,38.93932|13"}] }, { n: "广东", g: "113.394818,23.408004|8", cities: [{ n: "广州", g: "113.30765,23.120049|12" }, { n: "潮州", g: "116.630076,23.661812|13" }, { n: "东莞", g: "113.763434,23.043024|12" }, { n: "佛山", g: "113.134026,23.035095|13" }, { n: "河源", g: "114.713721,23.757251|12" }, { n: "惠州", g: "114.410658,23.11354|12" }, { n: "江门", g: "113.078125,22.575117|13" }, { n: "揭阳", g: "116.379501,23.547999|13" }, { n: "茂名", g: "110.931245,21.668226|13" }, { n: "梅州", g: "116.126403,24.304571|13" }, { n: "清远", g: "113.040773,23.698469|13" }, { n: "汕头", g: "116.72865,23.383908|13" }, { n: "汕尾", g: "115.372924,22.778731|14" }, { n: "韶关", g: "113.594461,24.80296|13" }, { n: "深圳", g: "114.025974,22.546054|12" }, { n: "阳江", g: "111.97701,21.871517|14" }, { n: "云浮", g: "112.050946,22.937976|13" }, { n: "湛江", g: "110.365067,21.257463|13" }, { n: "肇庆", g: "112.479653,23.078663|13" }, { n: "中山", g: "113.42206,22.545178|12" }, { n: "珠海", g: "113.562447,22.256915|13"}] }, { n: "广西", g: "108.924274,23.552255|7", cities: [{ n: "南宁", g: "108.297234,22.806493|12" }, { n: "百色", g: "106.631821,23.901512|13" }, { n: "北海", g: "109.122628,21.472718|13" }, { n: "崇左", g: "107.357322,22.415455|14" }, { n: "防城港", g: "108.351791,21.617398|15" }, { n: "桂林", g: "110.26092,25.262901|12" }, { n: "贵港", g: "109.613708,23.103373|13" }, { n: "河池", g: "108.069948,24.699521|14" }, { n: "贺州", g: "111.552594,24.411054|14" }, { n: "来宾", g: "109.231817,23.741166|14" }, { n: "柳州", g: "109.422402,24.329053|12" }, { n: "钦州", g: "108.638798,21.97335|13" }, { n: "梧州", g: "111.305472,23.485395|13" }, { n: "玉林", g: "110.151676,22.643974|14"}] }, { n: "贵州", g: "106.734996,26.902826|8", cities: [{ n: "贵阳", g: "106.709177,26.629907|12" }, { n: "安顺", g: "105.92827,26.228595|13" }, { n: "毕节地区", g: "105.300492,27.302612|14" }, { n: "六盘水", g: "104.852087,26.591866|13" }, { n: "铜仁地区", g: "109.196161,27.726271|14" }, { n: "遵义", g: "106.93126,27.699961|13" }, { n: "黔西南州", g: "104.900558,25.095148|11" }, { n: "黔东南州", g: "107.985353,26.583992|11" }, { n: "黔南州", g: "107.523205,26.264536|11"}] }, { n: "海南", g: "109.733755,19.180501|9", cities: [{ n: "海口", g: "110.330802,20.022071|13" }, { n: "白沙", g: "109.358586,19.216056|12" }, { n: "保亭", g: "109.656113,18.597592|12" }, { n: "昌江", g: "109.0113,19.222483|12" }, { n: "儋州", g: "109.413973,19.571153|13" }, { n: "澄迈", g: "109.996736,19.693135|13" }, { n: "东方", g: "108.85101,18.998161|13" }, { n: "定安", g: "110.32009,19.490991|13" }, { n: "琼海", g: "110.414359,19.21483|13" }, { n: "琼中", g: "109.861849,19.039771|12" }, { n: "乐东", g: "109.062698,18.658614|12" }, { n: "临高", g: "109.724101,19.805922|13" }, { n: "陵水", g: "109.948661,18.575985|12" }, { n: "三亚", g: "109.522771,18.257776|12" }, { n: "屯昌", g: "110.063364,19.347749|13" }, { n: "万宁", g: "110.292505,18.839886|13" }, { n: "文昌", g: "110.780909,19.750947|13" }, { n: "五指山", g: "109.51775,18.831306|13"}] }, { n: "河北", g: "115.661434,38.61384|7", cities: [{ n: "石家庄", g: "114.522082,38.048958|12" }, { n: "保定", g: "115.49481,38.886565|13" }, { n: "沧州", g: "116.863806,38.297615|13" }, { n: "承德", g: "117.933822,40.992521|14" }, { n: "邯郸", g: "114.482694,36.609308|13" }, { n: "衡水", g: "115.686229,37.746929|13" }, { n: "廊坊", g: "116.703602,39.518611|13" }, { n: "秦皇岛", g: "119.604368,39.945462|12" }, { n: "唐山", g: "118.183451,39.650531|13" }, { n: "邢台", g: "114.520487,37.069531|13" }, { n: "张家口", g: "114.893782,40.811188|13"}] }, { n: "河南", g: "113.486804,34.157184|7", cities: [{ n: "郑州", g: "113.649644,34.75661|12" }, { n: "安阳", g: "114.351807,36.110267|12" }, { n: "鹤壁", g: "114.29777,35.755426|13" }, { n: "焦作", g: "113.211836,35.234608|13" }, { n: "开封", g: "114.351642,34.801854|13" }, { n: "洛阳", g: "112.447525,34.657368|12" }, { n: "漯河", g: "114.046061,33.576279|13" }, { n: "南阳", g: "112.542842,33.01142|13" }, { n: "平顶山", g: "113.300849,33.745301|13" }, { n: "濮阳", g: "115.026627,35.753298|12" }, { n: "三门峡", g: "111.181262,34.78332|13" }, { n: "商丘", g: "115.641886,34.438589|13" }, { n: "新乡", g: "113.91269,35.307258|13" }, { n: "信阳", g: "114.085491,32.128582|13" }, { n: "许昌", g: "113.835312,34.02674|13" }, { n: "周口", g: "114.654102,33.623741|13" }, { n: "驻马店", g: "114.049154,32.983158|13"}] }, { n: "黑龙江", g: "128.047414,47.356592|6", cities: [{ n: "哈尔滨", g: "126.657717,45.773225|12" }, { n: "大庆", g: "125.02184,46.596709|12" }, { n: "大兴安岭地区", g: "124.196104,51.991789|10" }, { n: "鹤岗", g: "130.292472,47.338666|13" }, { n: "黑河", g: "127.50083,50.25069|14" }, { n: "鸡西", g: "130.941767,45.32154|13" }, { n: "佳木斯", g: "130.284735,46.81378|12" }, { n: "牡丹江", g: "129.608035,44.588521|13" }, { n: "七台河", g: "131.019048,45.775005|14" }, { n: "齐齐哈尔", g: "123.987289,47.3477|13" }, { n: "双鸭山", g: "131.171402,46.655102|13" }, { n: "绥化", g: "126.989095,46.646064|13" }, { n: "伊春", g: "128.910766,47.734685|14"}] }, { n: "湖北", g: "112.410562,31.209316|8", cities: [{ n: "武汉", g: "114.3162,30.581084|12" }, { n: "鄂州", g: "114.895594,30.384439|14" }, { n: "恩施", g: "109.517433,30.308978|14" }, { n: "黄冈", g: "114.906618,30.446109|14" }, { n: "黄石", g: "115.050683,30.216127|13" }, { n: "荆门", g: "112.21733,31.042611|13" }, { n: "荆州", g: "112.241866,30.332591|12" }, { n: "潜江", g: "112.768768,30.343116|13" }, { n: "神农架林区", g: "110.487231,31.595768|13" }, { n: "十堰", g: "110.801229,32.636994|13" }, { n: "随州", g: "113.379358,31.717858|13" }, { n: "天门", g: "113.12623,30.649047|13" }, { n: "仙桃", g: "113.387448,30.293966|13" }, { n: "咸宁", g: "114.300061,29.880657|13" }, { n: "襄樊", g: "112.176326,32.094934|12" }, { n: "孝感", g: "113.935734,30.927955|13" }, { n: "宜昌", g: "111.310981,30.732758|13"}] }, { n: "湖南", g: "111.720664,27.695864|7", cities: [{ n: "长沙", g: "112.979353,28.213478|12" }, { n: "常德", g: "111.653718,29.012149|12" }, { n: "郴州", g: "113.037704,25.782264|13" }, { n: "衡阳", g: "112.583819,26.898164|13" }, { n: "怀化", g: "109.986959,27.557483|13" }, { n: "娄底", g: "111.996396,27.741073|13" }, { n: "邵阳", g: "111.461525,27.236811|13" }, { n: "湘潭", g: "112.935556,27.835095|13" }, { n: "湘西州", g: "109.745746,28.317951|14" }, { n: "益阳", g: "112.366547,28.588088|13" }, { n: "永州", g: "111.614648,26.435972|13" }, { n: "岳阳", g: "113.146196,29.378007|13" }, { n: "张家界", g: "110.48162,29.124889|13" }, { n: "株洲", g: "113.131695,27.827433|13"}] }, { n: "江苏", g: "119.368489,33.013797|8", cities: [{ n: "南京", g: "118.778074,32.057236|12" }, { n: "常州", g: "119.981861,31.771397|12" }, { n: "淮安", g: "119.030186,33.606513|12" }, { n: "连云港", g: "119.173872,34.601549|12" }, { n: "南通", g: "120.873801,32.014665|12" }, { n: "苏州", g: "120.619907,31.317987|12" }, { n: "宿迁", g: "118.296893,33.95205|13" }, { n: "泰州", g: "119.919606,32.476053|13" }, { n: "无锡", g: "120.305456,31.570037|12" }, { n: "徐州", g: "117.188107,34.271553|12" }, { n: "盐城", g: "120.148872,33.379862|12" }, { n: "扬州", g: "119.427778,32.408505|13" }, { n: "镇江", g: "119.455835,32.204409|13"}] }, { n: "江西", g: "115.676082,27.757258|7", cities: [{ n: "南昌", g: "115.893528,28.689578|12" }, { n: "抚州", g: "116.360919,27.954545|13" }, { n: "赣州", g: "114.935909,25.845296|13" }, { n: "吉安", g: "114.992039,27.113848|13" }, { n: "景德镇", g: "117.186523,29.303563|12" }, { n: "九江", g: "115.999848,29.71964|13" }, { n: "萍乡", g: "113.859917,27.639544|13" }, { n: "上饶", g: "117.955464,28.457623|13" }, { n: "新余", g: "114.947117,27.822322|13" }, { n: "宜春", g: "114.400039,27.81113|13" }, { n: "鹰潭", g: "117.03545,28.24131|13"}] }, { n: "吉林", g: "126.262876,43.678846|7", cities: [{ n: "长春", g: "125.313642,43.898338|12" }, { n: "白城", g: "122.840777,45.621086|13" }, { n: "白山", g: "126.435798,41.945859|13" }, { n: "吉林市", g: "126.564544,43.871988|12" }, { n: "辽源", g: "125.133686,42.923303|13" }, { n: "四平", g: "124.391382,43.175525|12" }, { n: "松原", g: "124.832995,45.136049|13" }, { n: "通化", g: "125.94265,41.736397|13" }, { n: "延边", g: "129.485902,42.896414|13"}] }, { n: "辽宁", g: "122.753592,41.6216|8", cities: [{ n: "沈阳", g: "123.432791,41.808645|12" }, { n: "鞍山", g: "123.007763,41.118744|13" }, { n: "本溪", g: "123.778062,41.325838|12" }, { n: "朝阳", g: "120.446163,41.571828|13" }, { n: "大连", g: "121.593478,38.94871|12" }, { n: "丹东", g: "124.338543,40.129023|12" }, { n: "抚顺", g: "123.92982,41.877304|12" }, { n: "阜新", g: "121.660822,42.01925|14" }, { n: "葫芦岛", g: "120.860758,40.74303|13" }, { n: "锦州", g: "121.147749,41.130879|13" }, { n: "辽阳", g: "123.172451,41.273339|14" }, { n: "盘锦", g: "122.073228,41.141248|13" }, { n: "铁岭", g: "123.85485,42.299757|13" }, { n: "营口", g: "122.233391,40.668651|13"}] }, { n: "内蒙古", g: "114.415868,43.468238|5", cities: [{ n: "呼和浩特", g: "111.660351,40.828319|12" }, { n: "阿拉善盟", g: "105.695683,38.843075|14" }, { n: "包头", g: "109.846239,40.647119|12" }, { n: "巴彦淖尔", g: "107.423807,40.76918|12" }, { n: "赤峰", g: "118.930761,42.297112|12" }, { n: "鄂尔多斯", g: "109.993706,39.81649|12" }, { n: "呼伦贝尔", g: "119.760822,49.201636|12" }, { n: "通辽", g: "122.260363,43.633756|12" }, { n: "乌海", g: "106.831999,39.683177|13" }, { n: "乌兰察布", g: "113.112846,41.022363|12" }, { n: "锡林郭勒盟", g: "116.02734,43.939705|11" }, { n: "兴安盟", g: "122.048167,46.083757|11"}] }, { n: "宁夏", g: "106.155481,37.321323|8", cities: [{ n: "银川", g: "106.206479,38.502621|12" }, { n: "固原", g: "106.285268,36.021523|13" }, { n: "石嘴山", g: "106.379337,39.020223|13" }, { n: "吴忠", g: "106.208254,37.993561|14" }, { n: "中卫", g: "105.196754,37.521124|14"}] }, { n: "青海", g: "96.202544,35.499761|7", cities: [{ n: "西宁", g: "101.767921,36.640739|12" }, { n: "果洛州", g: "100.223723,34.480485|11" }, { n: "海东地区", g: "102.085207,36.51761|11" }, { n: "海北州", g: "100.879802,36.960654|11" }, { n: "海南州", g: "100.624066,36.284364|11" }, { n: "海西州", g: "97.342625,37.373799|11" }, { n: "黄南州", g: "102.0076,35.522852|11" }, { n: "玉树州", g: "97.013316,33.00624|14"}] }, { n: "山东", g: "118.527663,36.09929|8", cities: [{ n: "济南", g: "117.024967,36.682785|12" }, { n: "滨州", g: "117.968292,37.405314|12" }, { n: "东营", g: "118.583926,37.487121|12" }, { n: "德州", g: "116.328161,37.460826|12" }, { n: "菏泽", g: "115.46336,35.26244|13" }, { n: "济宁", g: "116.600798,35.402122|13" }, { n: "莱芜", g: "117.684667,36.233654|13" }, { n: "聊城", g: "115.986869,36.455829|12" }, { n: "临沂", g: "118.340768,35.072409|12" }, { n: "青岛", g: "120.384428,36.105215|12" }, { n: "日照", g: "119.50718,35.420225|12" }, { n: "泰安", g: "117.089415,36.188078|13" }, { n: "威海", g: "122.093958,37.528787|13" }, { n: "潍坊", g: "119.142634,36.716115|12" }, { n: "烟台", g: "121.309555,37.536562|12" }, { n: "枣庄", g: "117.279305,34.807883|13" }, { n: "淄博", g: "118.059134,36.804685|12"}] }, { n: "山西", g: "112.515496,37.866566|7", cities: [{ n: "太原", g: "112.550864,37.890277|12" }, { n: "长治", g: "113.120292,36.201664|12" }, { n: "大同", g: "113.290509,40.113744|12" }, { n: "晋城", g: "112.867333,35.499834|13" }, { n: "晋中", g: "112.738514,37.693362|13" }, { n: "临汾", g: "111.538788,36.099745|13" }, { n: "吕梁", g: "111.143157,37.527316|14" }, { n: "朔州", g: "112.479928,39.337672|13" }, { n: "忻州", g: "112.727939,38.461031|12" }, { n: "阳泉", g: "113.569238,37.869529|13" }, { n: "运城", g: "111.006854,35.038859|13"}] }, { n: "陕西", g: "109.503789,35.860026|7", cities: [{ n: "西安", g: "108.953098,34.2778|12" }, { n: "安康", g: "109.038045,32.70437|13" }, { n: "宝鸡", g: "107.170645,34.364081|12" }, { n: "汉中", g: "107.045478,33.081569|13" }, { n: "商洛", g: "109.934208,33.873907|13" }, { n: "铜川", g: "108.968067,34.908368|13" }, { n: "渭南", g: "109.483933,34.502358|13" }, { n: "咸阳", g: "108.707509,34.345373|13" }, { n: "延安", g: "109.50051,36.60332|13" }, { n: "榆林", g: "109.745926,38.279439|12"}] }, { n: "四川", g: "102.89916,30.367481|7", cities: [{ n: "成都", g: "104.067923,30.679943|12" }, { n: "阿坝州", g: "102.228565,31.905763|15" }, { n: "巴中", g: "106.757916,31.869189|14" }, { n: "达州", g: "107.494973,31.214199|14" }, { n: "德阳", g: "104.402398,31.13114|13" }, { n: "甘孜州", g: "101.969232,30.055144|15" }, { n: "广安", g: "106.63572,30.463984|13" }, { n: "广元", g: "105.819687,32.44104|13" }, { n: "乐山", g: "103.760824,29.600958|13" }, { n: "凉山州", g: "102.259591,27.892393|14" }, { n: "泸州", g: "105.44397,28.89593|14" }, { n: "南充", g: "106.105554,30.800965|13" }, { n: "眉山", g: "103.84143,30.061115|13" }, { n: "绵阳", g: "104.705519,31.504701|12" }, { n: "内江", g: "105.073056,29.599462|13" }, { n: "攀枝花", g: "101.722423,26.587571|14" }, { n: "遂宁", g: "105.564888,30.557491|12" }, { n: "雅安", g: "103.009356,29.999716|13" }, { n: "宜宾", g: "104.633019,28.769675|13" }, { n: "资阳", g: "104.63593,30.132191|13" }, { n: "自贡", g: "104.776071,29.359157|13"}] }, { n: "西藏", g: "89.137982,31.367315|6", cities: [{ n: "拉萨", g: "91.111891,29.662557|13" }, { n: "阿里地区", g: "81.107669,30.404557|11" }, { n: "昌都地区", g: "97.185582,31.140576|15" }, { n: "林芝地区", g: "94.349985,29.666941|11" }, { n: "那曲地区", g: "92.067018,31.48068|14" }, { n: "日喀则地区", g: "88.891486,29.269023|14" }, { n: "山南地区", g: "91.750644,29.229027|11"}] }, { n: "新疆", g: "85.614899,42.127001|6", cities: [{ n: "乌鲁木齐", g: "87.564988,43.84038|12" }, { n: "阿拉尔", g: "81.291737,40.61568|13" }, { n: "阿克苏地区", g: "80.269846,41.171731|12" }, { n: "阿勒泰地区", g: "88.137915,47.839744|13" }, { n: "巴音郭楞", g: "86.121688,41.771362|12" }, { n: "博尔塔拉州", g: "82.052436,44.913651|11" }, { n: "昌吉州", g: "87.296038,44.007058|13" }, { n: "哈密地区", g: "93.528355,42.858596|13" }, { n: "和田地区", g: "79.930239,37.116774|13" }, { n: "喀什地区", g: "75.992973,39.470627|12" }, { n: "克拉玛依", g: "84.88118,45.594331|13" }, { n: "克孜勒苏州", g: "76.137564,39.750346|11" }, { n: "石河子", g: "86.041865,44.308259|13" }, { n: "塔城地区", g: "82.974881,46.758684|12" }, { n: "图木舒克", g: "79.198155,39.889223|13" }, { n: "吐鲁番地区", g: "89.181595,42.96047|13" }, { n: "五家渠", g: "87.565449,44.368899|13" }, { n: "伊犁州", g: "81.297854,43.922248|11"}] }, { n: "云南", g: "101.592952,24.864213|7", cities: [{ n: "昆明", g: "102.714601,25.049153|12" }, { n: "保山", g: "99.177996,25.120489|13" }, { n: "楚雄州", g: "101.529382,25.066356|13" }, { n: "大理州", g: "100.223675,25.5969|14" }, { n: "德宏州", g: "98.589434,24.44124|14" }, { n: "迪庆州", g: "99.713682,27.831029|14" }, { n: "红河州", g: "103.384065,23.367718|11" }, { n: "丽江", g: "100.229628,26.875351|13" }, { n: "临沧", g: "100.092613,23.887806|14" }, { n: "怒江州", g: "98.859932,25.860677|14" }, { n: "普洱", g: "100.980058,22.788778|14" }, { n: "曲靖", g: "103.782539,25.520758|12" }, { n: "昭通", g: "103.725021,27.340633|13" }, { n: "文山", g: "104.089112,23.401781|14" }, { n: "西双版纳", g: "100.803038,22.009433|13" }, { n: "玉溪", g: "102.545068,24.370447|13"}] }, { n: "浙江", g: "119.957202,29.159494|8", cities: [{ n: "杭州", g: "120.219375,30.259244|12" }, { n: "湖州", g: "120.137243,30.877925|12" }, { n: "嘉兴", g: "120.760428,30.773992|13" }, { n: "金华", g: "119.652576,29.102899|12" }, { n: "丽水", g: "119.929576,28.4563|13" }, { n: "宁波", g: "121.579006,29.885259|12" }, { n: "衢州", g: "118.875842,28.95691|12" }, { n: "绍兴", g: "120.592467,30.002365|13" }, { n: "台州", g: "121.440613,28.668283|13" }, { n: "温州", g: "120.690635,28.002838|12" }, { n: "舟山", g: "122.169872,30.03601|13"}]}], other: [{ n: "香港", g: "114.186124,22.293586|11" }, { n: "澳门", g: "113.557519,22.204118|13" }, { n: "台湾", g: "120.961454,23.80406|8"}] };
var EnmAlarmLevel = { NoAlarm: 0, Critical: 1, Major: 2, Minor: 3, Hint: 4 };
var EnmMapType = { GPS: 0, Other: 1 };
var NavMap = {
    map: null,
    infoWnd: null,
    editWnd: null,
    translateStations: null,
    openStation: null,
    cityCenter: { lng: 121.487899, lat: 31.249162, level: 12 },
    render: function() {
        this.map = new BMap.Map("mapContainer");
        this.map.centerAndZoom(new BMap.Point(this.cityCenter.lng, this.cityCenter.lat), this.cityCenter.level);
        this.map.addControl(new BMap.NavigationControl());
        this.map.addControl(new BMap.ScaleControl());
        this.map.addControl(new BMap.OverviewMapControl());
        this.map.addControl(new BMap.MapTypeControl({ mapTypes: [BMAP_NORMAL_MAP, BMAP_SATELLITE_MAP, BMAP_HYBRID_MAP] }));
        this.map.enableScrollWheelZoom();
        var contextMenu = new BMap.ContextMenu();
        var txtMenuItem = [
            { text: '放大', callback: function() { NavMap.map.zoomIn() } },
            { text: '缩小', callback: function() { NavMap.map.zoomOut() } },
            { text: '在此添加标记', callback: function(p) {
                NavMap.initWnd(function() {
                    var marker = NavMap.createMarker(p, null);
                    NavMap.map.addOverlay(marker);
                    NavMap.editWnd.open(marker);
                });
            }
            }
            ];
        for (var i = 0; i < txtMenuItem.length; i++) {
            contextMenu.addItem(new BMap.MenuItem(txtMenuItem[i].text, txtMenuItem[i].callback, 100));
            if (i == 1) { contextMenu.addSeparator(); }
        }
        this.map.addContextMenu(contextMenu);
        this.map.addEventListener("dragstart", function() {
            NavMap.initWnd(function() { NavMap.map.clearOverlays(); });
        });
        this.map.addEventListener("dragend", function() {
            NavMap.createViewMarkers();
        });
        this.map.addEventListener("zoomstart", function() {
            NavMap.initWnd(function() { NavMap.map.clearOverlays(); });
        });
        this.map.addEventListener("zoomend", function() {
            NavMap.createViewMarkers();
        });
        this.map.addEventListener("resize", function() {
            NavMap.createViewMarkers();
        });

        var localCity = new BMap.LocalCity();
        localCity.get(function(point) {
            NavMap.setLocation(point.center, point.level, EnmMapType.Other);
        });
    },
    buildCityCenterTree: function(el) {
        var root = el.getRootNode();
        if (root) {
            for (var i = 0; i < cityCenter.municipalities.length; i++) {
                var node = new Ext.tree.TreeNode({
                    id: cityCenter.municipalities[i].g,
                    text: cityCenter.municipalities[i].n,
                    singleClickExpand: true,
                    iconCls: "icon-building"
                });
                root.appendChild(node);
            }

            for (var i = 0; i < cityCenter.other.length; i++) {
                var node = new Ext.tree.TreeNode({
                    id: cityCenter.other[i].g,
                    text: cityCenter.other[i].n,
                    singleClickExpand: true,
                    iconCls: "icon-building"
                });
                root.appendChild(node);
            }

            for (var i = 0; i < cityCenter.provinces.length; i++) {
                var node = new Ext.tree.TreeNode({
                    id: cityCenter.provinces[i].g,
                    text: cityCenter.provinces[i].n,
                    singleClickExpand: true,
                    iconCls: "icon-building"
                });

                for (var j = 0; j < cityCenter.provinces[i].cities.length; j++) {
                    var child = new Ext.tree.TreeNode({
                        id: cityCenter.provinces[i].cities[j].g,
                        text: cityCenter.provinces[i].cities[j].n,
                        singleClickExpand: true,
                        iconCls: "icon-building"
                    });
                    node.appendChild(child);
                }
                root.appendChild(node);
            }
        }
    },
    setMapPanelSize: function(el) {
        var container = Ext.get("mapContainer");
        if (container) {
            container.setWidth(el.getWidth())
            container.setHeight(el.getHeight() - 50);
        }
    },
    onNavTreeItemClick: function(el, e) {
        if (!el.parentNode) { return false; }
        var cc = el.id.split("|");
        var g = cc[0].split(",");
        var level = parseInt(cc[1], 10);
        var lng = parseFloat(g[0]);
        var lat = parseFloat(g[1]);
        this.setLocation(new BMap.Point(lng, lat), level, EnmMapType.Other);
    },
    createViewMarkers: function() {
        this.stopTask(0);
        this.initWnd(function() { NavMap.map.clearOverlays(); });
        var bounds = this.map.getBounds();
        var sw = bounds.getSouthWest();
        var ne = bounds.getNorthEast();
        //兼容GPS经纬度范围，需要减0.02和0.01
        X.NavMaps.GetViewStations(sw.lng - 0.02, ne.lng, sw.lat - 0.01, ne.lat, {
            success: function(result) {
                if (!Ext.isEmpty(result, false)) {
                    NavMap.translateStations = [];
                    var stations = Ext.util.JSON.decode(result, true);
                    if (stations && stations.length > 0) {
                        for (var i = 0; i < stations.length; i++) {
                            if (stations[i].mapType == EnmMapType.Other) { NavMap.addMarker(stations[i]); } else { NavMap.translateStations.push(stations[i]); }
                        }
                        if (NavMap.translateStations.length > 0) { NavMap.translateGPS(); } else { NavMap.startTask(0); }
                    }
                }
            }
        });
    },
    addMarker: function(data) {
        var point = new BMap.Point(data.lng, data.lat);
        var marker = this.createMarker(point, data);
        this.map.addOverlay(marker);
        if (this.openStation) {
            if (data.lscId == this.openStation.lscId && data.staId == this.openStation.staId) {
                this.infoWnd.open(marker);
                this.openStation = null;
            }
        }
    },
    translateGPS: function() {
        if (this.translateStations.length > 0) {
            var points = [];
            var maxCnt = 20;
            while (true) {
                if (NavMap.translateStations.length == 0 || points.length >= maxCnt) { break; }
                var data = NavMap.translateStations.shift();
                if (data) {
                    var point = new BMap.Point(data.lng, data.lat);
                    point._data = data;
                    points.push(point);
                }
            }

            if (points.length > 0) {
                BMap.Convertor.translate(points, 0, function(xyResults) {
                    if (xyResults) {
                        var xyResult = null;
                        for (var index = 0; index < xyResults.length; index++) {
                            xyResult = xyResults[index];
                            if (xyResult.error != 0) { continue; }
                            var rp = new BMap.Point(xyResult.x, xyResult.y);
                            var op = points[index];
                            if (op && op._data) {
                                op._data.mapType = EnmMapType.Other;
                                op._data.lng = rp.lng;
                                op._data.lat = rp.lat;
                                NavMap.addMarker(op._data);
                            }
                        }
                    }
                    if (NavMap.translateStations.length > 0) { window.setTimeout(function() { NavMap.translateGPS(); }, 1000); } else { NavMap.startTask(0); }
                });
            }
        }
    },
    createMarker: function(point, data) {
        var tp = data || { staName: "undefined" };
        var marker = new BMap.Marker(point, { icon: new BMap.Icon(this.getIconUrl(EnmAlarmLevel.NoAlarm), new BMap.Size(16, 16)) });
        var label = new BMap.Label(tp.staName, { "offset": new BMap.Size(16, 0) });
        label.setStyle({
            borderColor: "#808080",
            color: "#333333",
            cursor: "pointer"
        });
        marker.setLabel(label);
        label._marker = marker;
        if (data) { marker._data = data; }
        marker.addEventListener("click", function() {
            NavMap.initWnd();
            NavMap.infoWnd.open(this);
        });
        label.addEventListener("click", function() {
            NavMap.fireEvent(this._marker, "click");
        });
        return marker;
    },
    updateViewMarkers: function() {
        var bounds = this.map.getBounds();
        var sw = bounds.getSouthWest();
        var ne = bounds.getNorthEast();
        //兼容GPS经纬度范围，需要减0.02和0.01
        X.NavMaps.GetMarkers(sw.lng - 0.02, ne.lng, sw.lat - 0.01, ne.lat, {
            success: function(result) {
                if (!Ext.isEmpty(result, false)) {
                    var data = Ext.util.JSON.decode(result, true);
                    if (data) {
                        var alarmCnt = Ext.get("div-alarm-cnt");
                        var alarm1 = Ext.get("div-alarm-1");
                        var alarm2 = Ext.get("div-alarm-2");
                        var alarm3 = Ext.get("div-alarm-3");
                        var alarm4 = Ext.get("div-alarm-4");
                        if (alarmCnt) { alarmCnt.update(String.format("({0})", data.AL1 + data.AL2 + data.AL3 + data.AL4)); }
                        if (alarm1) { alarm1.update(String.format("({0})", data.AL1)); }
                        if (alarm2) { alarm2.update(String.format("({0})", data.AL2)); }
                        if (alarm3) { alarm3.update(String.format("({0})", data.AL3)); }
                        if (alarm4) { alarm4.update(String.format("({0})", data.AL4)); }

                        var markers = NavMap.map.getOverlays();
                        for (var i = 0; i < markers.length; i++) {
                            if (markers[i] instanceof BMap.Marker) {
                                if (markers[i]._data) {
                                    var index = encodeURIComponent(String.format("m_{0}_{1}", markers[i]._data.lscId, markers[i]._data.staName));
                                    var c = data.Stations[index];
                                    if (c)
                                        markers[i].setIcon(new BMap.Icon(NavMap.getIconUrl(c), new BMap.Size(16, 16)));
                                    else
                                        markers[i].setIcon(new BMap.Icon(NavMap.getIconUrl(EnmAlarmLevel.NoAlarm), new BMap.Size(16, 16)));
                                }
                            }
                        }
                    }
                }
            }
        });
    },
    updateMarker: function(marker, data) {
        marker.setIcon(new BMap.Icon(NavMap.getIconUrl(data.maxAL), new BMap.Size(16, 16)));
        marker.getLabel().setContent(data.staName);
        marker._data = data;
    },
    createInfoWindow: function() {
        var infoWnd = document.getElementById("infoWnd");
        var infoWnd_content = document.getElementById("infoWnd_content");
        var infoWnd_content_container = document.getElementById("infoWnd_content_container");
        var infoWnd_title = document.getElementById("infoWnd_title");
        var infoWnd_title_container = document.getElementById("infoWnd_title_container");
        var infoWnd_title_container_name = document.getElementById("infoWnd_title_container_name");
        var infoWnd_content_control_delete = document.getElementById("infoWnd_content_control_delete");
        var infoWnd_content_control_edit = document.getElementById("infoWnd_content_control_edit");
        var searchInfoWindow = new BMapLib.SearchInfoWindow(this.map, infoWnd_content, {
            title: infoWnd_title.innerHTML,
            width: 300,
            height: 150,
            panel: null,
            enableAutoPan: true,
            searchTypes: []
        });

        searchInfoWindow.setWndTitle = function(title) {
            infoWnd_title_container.setAttribute("title", title);
            infoWnd_title_container_name.innerHTML = NavMap.getShortTitle(title, 20);
            this.setTitle(infoWnd_title.innerHTML);
        }
        searchInfoWindow.setWndContent = function(content) {
            infoWnd_content_container.innerHTML = content;
            this.setContent(infoWnd_content);
        }
        searchInfoWindow.setWndWaitting = function() {
            infoWnd_content_container.innerHTML = "<div class='waitting'>数据加载中...</div>";
            this.setContent(infoWnd_content);
        }
        searchInfoWindow.showDetailWnd = function(level) {
            if (this._marker._data) {
                X.NavMaps.ShowDetailWnd(this._marker._data.lscId, this._marker._data.staName, level);
            }
        }
        searchInfoWindow.showDevWnd = function() {
            if (this._marker._data) {
                X.NavMaps.ShowDevWnd(this._marker._data.lscId,this._marker._data.staId, this._marker._data.staName);
            }
        }
        searchInfoWindow.addEventListener("open", function() {
            this._marker.getLabel().hide();
            this.setWndTitle(this._marker._data.staName);
            this.setWndWaitting();
            X.NavMaps.GetStationDetail(this._marker._data.lscId, this._marker._data.staId, {
                success: function(result) {
                    if (!Ext.isEmpty(result, false)) {
                        var station = Ext.util.JSON.decode(result, true);
                        if (station) { NavMap.infoWnd.setWndContent(NavMap.getContentWnd(station)); }
                    }
                }
            });
        });
        searchInfoWindow.addEventListener("close", function() {
            if (this._marker) { this._marker.getLabel().show(); }
        });
        NavMap.addEventListener(infoWnd_content_control_delete, "click", function() {
            X.NavMaps.UpdateMarker(NavMap.infoWnd._marker._data.lscId, NavMap.infoWnd._marker._data.staId, NavMap.infoWnd._marker._data.lscId, EnmMapType.GPS, NavMap.infoWnd._marker._data.staId, 0, 0, "", {
                success: function(result) {
                    NavMap.infoWnd.close();
                    NavMap.map.removeOverlay(NavMap.infoWnd._marker);
                }
            });
        });
        NavMap.addEventListener(infoWnd_content_control_edit, "click", function() {
            if (NavMap.infoWnd && NavMap.editWnd) {
                NavMap.infoWnd.close();
                NavMap.editWnd.open(NavMap.infoWnd._marker);
            }
            return false;
        });
        return searchInfoWindow;
    },
    createEditWindow: function() {
        var editWnd_content = document.getElementById("editWnd_content");
        var editWnd_title = document.getElementById("editWnd_title");
        var editWnd_title_container = document.getElementById("editWnd_title_container");
        var btnSave = document.getElementById("btnSave");
        var btnCancel = document.getElementById("btnCancel");
        var searchEditWindow = new BMapLib.SearchInfoWindow(this.map, editWnd_content, {
            title: editWnd_title.innerHTML,
            width: 300,
            height: 165,
            panel: null,
            enableAutoPan: true,
            searchTypes: []
        });

        searchEditWindow.addEventListener("open", function() {
            if (this._marker) {
                this._marker.getLabel().hide();
                var point = this._marker.getPosition();
                us_editWnd_lng.setValue(point.lng);
                us_editWnd_lat.setValue(point.lat);
                if (this._marker._data) {
                    StaComboBox.store._staId = this._marker._data.staId;
                    LscsComboBox.setValueAndFireSelect(String.format("{0}&{1}", this._marker._data.lscId, this._marker._data.groupId));
                    us_editWnd_add.setValue(this._marker._data.address);
                }
                else {
                    var gc = new BMap.Geocoder();
                    gc.getLocation(point, function(rs) {
                        if (rs) {
                            var addComp = rs.addressComponents;
                            us_editWnd_add.setValue(String.format("{0}{1}{2}{3}{4}", addComp.province, addComp.city, addComp.district, addComp.street, addComp.streetNumber));
                        }
                    });
                }
                btnSave.disabled = false;
            }
        });
        searchEditWindow.addEventListener("close", function() {
            if (this._marker) {
                if (!this._marker._data) { NavMap.map.removeOverlay(this._marker); } else { this._marker.getLabel().show(); }
            }
        });
        NavMap.addEventListener(btnSave, "click", function() {
            var ids = LscsComboBox.getValue().split("&");
            var save = { lscId: ids[0], lscName: LscsComboBox.getText(), groupId: ids[1], staId: StaComboBox.getValue(), staName: StaComboBox.getText(), mapType: EnmMapType.Other, lng: us_editWnd_lng.getRawValue(), lat: us_editWnd_lat.getRawValue(), address: us_editWnd_add.getRawValue() };
            if (Ext.isEmpty(save.lscId, false)
            || Ext.isEmpty(save.staId, false)
            || Ext.isEmpty(save.lng, false)
            || Ext.isEmpty(save.lat, false)) {
                alert("提示：数据无效，保存失败！");
                return false;
            }

            this.disabled = true;
            var olscId = NavMap.editWnd._marker._data ? NavMap.editWnd._marker._data.lscId : save.lscId;
            var ostaId = NavMap.editWnd._marker._data ? NavMap.editWnd._marker._data.staId : save.staId;
            X.NavMaps.UpdateMarker(olscId, ostaId, save.lscId, save.staId, save.mapType, save.lng, save.lat, save.address, {
                success: function(result) {
                    NavMap.updateMarker(NavMap.editWnd._marker, save);
                    NavMap.editWnd.close();
                    if (NavMap.infoWnd) { NavMap.infoWnd.open(NavMap.editWnd._marker); }
                }
            });
        });
        NavMap.addEventListener(btnCancel, "click", function() {
            NavMap.editWnd.close();
            return false;
        });
        return searchEditWindow;
    },
    showTotalWnd: function(level) {
        X.NavMaps.ShowTotalWnd(level);
    },
    getIconUrl: function(level) {
        switch (level) {
            case EnmAlarmLevel.NoAlarm:
                return "Resources/images/Baidu/Map/labAL0.gif";
            case EnmAlarmLevel.Critical:
                return "Resources/images/Baidu/Map/labAL1.gif";
            case EnmAlarmLevel.Major:
                return "Resources/images/Baidu/Map/labAL2.gif";
            case EnmAlarmLevel.Minor:
                return "Resources/images/Baidu/Map/labAL3.gif";
            case EnmAlarmLevel.Hint:
                return "Resources/images/Baidu/Map/labAL4.gif";
            default:
                return "Resources/images/Baidu/Map/labAL0.gif";
        }
    },
    getShortTitle: function(title, maxLength) {
        if (title.length * 2 < maxLength) { return title; }
        var temp1 = title.replace(/[^\x00-\xff]/g, "^^");
        var temp2 = temp1.substring(0, maxLength);
        var hanzi_num = (temp2.split("\^").length - 1) / 2;
        maxLength = maxLength - hanzi_num;
        var res = title.substring(0, maxLength);
        if (maxLength < title.length) { return String.format("{0}...", res); } else { return res; }
    },
    getContentWnd: function(station) {
        return String.format("<div class=\"contentWndBox\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"content\"><tr><td valign=\"top\" class=\"title\">Lsc：</td><td valign=\"top\" title=\"{0}\">{0}</td></tr><tr><td valign=\"top\" class=\"title\">局站：</td><td valign=\"top\" title=\"{1}\">{1}</td></tr><tr><td valign=\"top\" class=\"title\">类型：</td><td valign=\"top\" title=\"{2}\">{2}</td></tr><tr><td valign=\"top\" class=\"title\">特征：</td><td valign=\"top\" title=\"{3}\">{3}</td></tr><tr><td valign=\"top\" class=\"title\">地址：</td><td valign=\"top\" title=\"{4}\" style=\"height:30px\">{5}</td></tr></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"2\" class=\"alarm\"><tr><td class=\"title\">一级告警：</td><td><a href=\"javascript:void(0)\" onclick=\"javascript:NavMap.infoWnd.showDetailWnd(EnmAlarmLevel.Critical);return false\">{6}&nbsp;条»</a></td><td class=\"title\">二级告警：</td><td><a href=\"javascript:void(0)\" onclick=\"javascript:NavMap.infoWnd.showDetailWnd(EnmAlarmLevel.Major);return false\">{7}&nbsp;条»</a></td></tr><tr><td class=\"title\">三级告警：</td><td><a href=\"javascript:void(0)\" onclick=\"javascript:NavMap.infoWnd.showDetailWnd(EnmAlarmLevel.Minor);return false\">{8}&nbsp;条»</a></td><td class=\"title\">四级告警：</td><td><a href=\"javascript:void(0)\" onclick=\"javascript:NavMap.infoWnd.showDetailWnd(EnmAlarmLevel.Hint);return false\">{9}&nbsp;条»</a></td></tr></table></div></div>", station.lscName, station.staName, station.staTypeName, station.staFeatureName, station.address, NavMap.getShortTitle(station.address, 82), station.AL1, station.AL2, station.AL3, station.AL4);
    },
    mapFilterChange: function(el, newValue, oldValue) {
        delete el._filterData;
        delete el._filterIndex;
    },
    mapFilterSearch: function(el, e) {
        var tt = TreeTriggerField,
        text = tt.getRawValue();
        if (Ext.isEmpty(text, false)) { return false; }

        if (tt._filterData != null && tt._filterIndex != null) {
            var index = tt._filterIndex + 1;
            var nodes = tt._filterData;
            if (index >= nodes.length) { index = 0; }
            this.openStation = { lscId: nodes[index].lscId, staId: nodes[index].staId };
            this.setLocation(new BMap.Point(nodes[index].lng, nodes[index].lat), 18, nodes[index].mapType);
            tt._filterIndex = index;
        }
        else {
            X.NavMaps.GetFilterStations(text, {
                success: function(result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes) {
                            var len = nodes.length;
                            if (len > 0) {
                                NavMap.openStation = { lscId: nodes[0].lscId, staId: nodes[0].staId };
                                NavMap.setLocation(new BMap.Point(nodes[0].lng, nodes[0].lat), 18, nodes[0].mapType);
                                tt._filterData = nodes;
                                tt._filterIndex = 0;
                            }
                        }
                    }
                }
            });
        }
    },
    setLocation: function(point, level, mapType) {
        var curll = this.map.getZoom();
        this.map.centerAndZoom(point, level);
        if (curll == level) { this.createViewMarkers(); }
    },
    openMarker: function(station) {
        if (station) {
            var markers = this.map.getOverlays();
            for (var i = 0; i < markers.length; i++) {
                if (markers[i] instanceof BMap.Marker) {
                    if (markers[i]._data
                    && markers[i]._data.lscId == station.lscId
                    && markers[i]._data.staId == station.staId) {
                        this.fireEvent(markers[i], "click");
                        this.openStation = null;
                        break;
                    }
                }
            }
        }
    },
    initWnd: function(callback) {
        if (this.infoWnd) { this.infoWnd.close(); } else { this.infoWnd = this.createInfoWindow(); }
        if (this.editWnd) { this.editWnd.close(); } else { this.editWnd = this.createEditWindow(); }
        if (callback && typeof (callback) == "function") { callback(); }
    },
    startTask: function(index) {
        if (index >= NavMapsTaskManager.tasks.length) { return false; }
        if (NavMapsTaskManager.tasks[index]._running == null) { NavMapsTaskManager.tasks[index]._running = false; }
        if (!NavMapsTaskManager.tasks[index]._running) {
            NavMapsTaskManager.startTask(index);
            NavMapsTaskManager.tasks[index]._running = true;
        }
    },
    stopTask: function(index) {
        if (index >= NavMapsTaskManager.tasks.length) { return false; }
        if (NavMapsTaskManager.tasks[index]._running == null) { NavMapsTaskManager.tasks[index]._running = false; }
        if (NavMapsTaskManager.tasks[index]._running) {
            NavMapsTaskManager.stopTask(index);
            NavMapsTaskManager.tasks[index]._running = false;
        }
    },
    addEventListener: function(instance, eventName, handler) {
        if (instance.addEventListener) {
            instance.addEventListener(eventName, handler, false);
        }
        else if (instance.attachEvent) {
            instance.attachEvent("on" + eventName, handler);
        }
        else {
            instance["on" + eventName] = handler;
        }
    },
    fireEvent: function(instance, eventName) {
        eventName.indexOf("on") != 0 && (eventName = "on" + eventName);
        instance.dispatchEvent(eventName);
    }
};
/****************************************************End****************************************************/