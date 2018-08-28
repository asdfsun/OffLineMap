
var mapObj = {
    //初始化参数
    init: function (options) {
        options = $.extend({
            defaultCity: '北京市',
            isOnLine: 1,     //1:在线;0:离线
            mapTilesUrl: "", //空值表示默认路径
            customSetting: false,
            customPoint: null,
            setMapZoom: 12,
            mapContainerId: '地图容器ID',
            securitySpotData: '渲染点数据源(每个点的坐标(经纬度),点内安检机数量)',
            enableMapClick: false,
            scaleControl: false,    //比例尺控件 默认禁用
            overviewMapControl: false,  //缩略图控件 默认禁用
            cityChangeBefore: function () { //切换城市之前事件
                alert('切换城市之前事件');
            },
            cityChangeAfter: function () { //切换城市之后事件
                alert('切换城市之后事件');
            },
            setMarkCallback: function () {
                alert('点击坐标之后事件');
            }
        }, options);
        this.options = options;
        //加载默认图标路径
        var href = $("script[src$='supervisionMap/supervisionMap.js']").attr("src");
        this.options.imgUrlBig = href.replace("supervisionMap.js", "mapIcon/da.png");
        this.options.imgUrlMiddle = href.replace("supervisionMap.js", "mapIcon/zhong.png");
        this.options.imgUrlSmall = href.replace("supervisionMap.js", "mapIcon/xiao.png");
        this.options.imgUrlZero = href.replace("supervisionMap.js", "mapIcon/e0.png");
        this.options.imgUrlSuperBig = href.replace("supervisionMap.js", "mapIcon/a.png");
        this.initMap();
        return this;
    },
    //初始化地图
    initMap: function () {
        var options = this.options;
        if (options.isOnLine == 0) {
            if (options.mapTilesUrl.trim().length > 0) {
                var outputPath = options.mapTilesUrl;
                var fromat = ".png";    //格式
                var tileLayer = new BMap.TileLayer();
                tileLayer.getTilesUrl = function (tileCoord, zoom) {
                    var x = tileCoord.x;
                    var y = tileCoord.y;
                    var url = outputPath + zoom + '/' + x + '/' + y + fromat;
                    return url;
                }
                var tileMapType = new BMap.MapType('tileMapType', tileLayer);
                var map = new BMap.Map(options.mapContainerId, { enableMapClick: options.enableMapClick, mapType: tileMapType, minZoom: 10, maxZoom: 11 });
            } else {
                var map = new BMap.Map(options.mapContainerId, { enableMapClick: options.enableMapClick, minZoom: 10, maxZoom: 11 });
            }
        }
        else {
            // 创建Map实例,并根据参数选择构造地图时,是否关闭地图可点功能
            var map = new BMap.Map(options.mapContainerId, { enableMapClick: options.enableMapClick });
        }

        // 初始化地图,设置中心点坐标和地图缩放级别   
        if (options.customSetting) {
            map.centerAndZoom(options.customPoint, options.setMapZoom)
        } else {
            options.defaultCity != '北京市' ? map.centerAndZoom(options.defaultCity, options.setMapZoom) : map.centerAndZoom(new BMap.Point(116.403569, 39.924075), options.setMapZoom);
        }
        map.addControl(new BMap.NavigationControl({
            type: BMAP_NAVIGATION_CONTROL_LARGE, //表示显示完整的平移缩放控件。  
            //type: BMAP_NAVIGATION_CONTROL_SMALL, //表示显示小型的平移缩放控件。  
            //type: BMAP_NAVIGATION_CONTROL_PAN, //表示只显示控件的平移部分功能。  
            //type: BMAP_NAVIGATION_CONTROL_ZOOM, //表示只显示控件的缩放部分功能。  
            //anchor: BMAP_ANCHOR_TOP_LEFT, //表示控件定位于地图的左上角。  
            //anchor: BMAP_ANCHOR_TOP_RIGHT, //表示控件定位于地图的右上角。  
            //anchor: BMAP_ANCHOR_BOTTOM_LEFT, //表示控件定位于地图的左下角。  
            anchor: BMAP_ANCHOR_BOTTOM_RIGHT, //表示控件定位于地图的右下角。  
            //offset: new BMap.Size(0, 0)
        }));
        map.enableKeyboard();       // 启用键盘操作。
        map.enableScrollWheelZoom();//启用滚轮放大缩小，默认禁用
        //map.disableDragging();    //禁止拖拽
        map.enableContinuousZoom(); //启用地图惯性拖拽，默认禁用
        map.disableDoubleClickZoom();//双击缩放禁用
        options.scaleControl && map.addControl(new BMap.ScaleControl());//比例尺控件
        options.overviewMapControl && map.addControl(new BMap.OverviewMapControl());//缩略图控件
        options.map = map;
        //渲染安检点
        this.renderSecuritySpot();
        $('.BMap_cpyCtrl span').hide();//取消版权文字
        return this;
    },
    //增加标识控件
    addSecuritySignControl: function () {
        var options = this.options;
        var map = options.map;
        //安检点标识控件
        function SecuritySignControl() { //定义一个控件类
            // 设置默认停靠位置和偏移量
            this.defaultAnchor = BMAP_ANCHOR_BOTTOM_RIGHT;
            this.defaultOffset = new BMap.Size(10, 10);
        }

        SecuritySignControl.prototype = new BMap.Control();// 通过JavaScript的prototype属性继承于BMap.Control
        // 自定义控件必须实现initialize方法，并且将控件的DOM元素返回
        SecuritySignControl.prototype.initialize = function (map) {// 在本方法中创建个div元素作为控件的容器，并将其添加到地图容器中
            var style = {// 创建元素样式
                supervisionMap_sample: 'position: absolute;background: #ffffff;border: 1px solid #c4c7cc;font-size: 12px;box-shadow: 1px 2px 1px rgba(0, 0, 0, .15);',
                liStyle: 'float: left;width: 40px;height: 50px;text-align: center;letter-spacing: normal;',
                iStyle: 'margin: 4px 4px -4px;display: block;width: 30px;height: 30px;'
            };
            // 创建DOM元素
            var div = $('<div style="' + style.supervisionMap_sample + '"><ul style="height: 100%;"><li style="' + style.liStyle + '"><p style="margin: 7px;">设备数量</p></li><li style="' + style.liStyle + '"><i style="' + style.iStyle + 'background: url(' + options.imgUrlBig + ') no-repeat center;"></i>1000+</li><li style="' + style.liStyle + '"><i style="' + style.iStyle + 'background: url(' + options.imgUrlMiddle + ') no-repeat center;"></i>100+</li><li style="' + style.liStyle + '"><i style="' + style.iStyle + 'background: url(' + options.imgUrlSmall + ') no-repeat center;"></i>10+</li></ul></div>')[0];
            // 添加DOM元素到地图中
            map.getContainer().appendChild(div);
            // 将DOM元素返回
            return div;
        };
        // 创建控件实例
        var mySecuritySignControl = new SecuritySignControl();// 添加到地图当中
        map.addControl(mySecuritySignControl);
    },
    //渲染安检点
    renderSecuritySpot: function () {
        var that = this;
        var options = this.options;
        //var map = options.map;
        var data = options.securitySpotData;
        for (var i = data.length-1; i >=0; i--) {//循环添加
            that.addMarker(data[i],i);
        }
    },
    addMarker: function (data,zindex) {
        var that = this;
        var options = this.options;
        var map = options.map;
        var image = new Image();
        image.src = data.securityImg;
        var a = 24;
        var b = 35;
        var icon = new BMap.Icon(data.securityImg, new BMap.Size(a, b), {
            anchor: new BMap.Size(12, 25)
            //, offset: new BMap.Size(0, 0)
            //, imageOffset: new BMap.Size(0, 10)
        });
        var myClickIcon = icon;
        var myIcon = icon;
        var label;
        //生成标签 设置标签位置
        if (parseInt(data.label) > 9) {
            if (parseInt(data.label) > 99)
            {
                data.label ="99+" ;
                label = new BMap.Label(data.label, {
                    offset: new BMap.Size(0, 3)
                });
            }
            else
            {
                //data.label = data.label;
                label = new BMap.Label(data.label, {
                    offset: new BMap.Size(3, 3)
                });
            }
        }
        else {
            label = new BMap.Label(data.label, {offset: new BMap.Size(7, 3)
            });
        }

        label.setStyle({
            background: 'none', color: '#fff', border: 'none'
        });
        //生成图标坐标
        var point = new BMap.Point(data.lng, data.lat);
        //添加坐标点属性,标签 注册回调事件
        var marker = new BMap.Marker(point, { icon: myIcon });
        marker.setZIndex(-zindex);
        marker.setLabel(label);
        // marker.number = number;//添加样式标识
        marker.myIcon = myIcon;//添加样式Url
        marker.myClickIcon = myClickIcon;//添加点击样式Url

        marker.addEventListener("click", function () {
            var p = marker.getPosition();//获取marker的位置
            that.mapSearch(p.lng, p.lat);//搜索并移动到marker的位置

            //$("#supervisionPointInfo2 input").prop("c.show();hecked", false);
            //$("#supervisionPointInfo2 label").removeClass("checked");
            //$("#supervisionPointInfo2 .radioBtn").removeClass("liHover");
            //$("#supervisionPointInfo2 .radioBtn").find("input").prop("checked", true).siblings("label").removeClass("checked");
            //$("#supervisionPointInfo2 .radioBtn").removeClass("liHover");           
            //$("#supervisionPointInfo .radioBtn").find("input").prop("checked", true).siblings("label").removeClass("checked");
            //$("#supervisionPointInfo .radioBtn").removeClass("liHover");
            //var isvalue = false;
            //if ($('#myCollection').hasClass('focus')) {
            //    $("#supervisionPointInfo .radioBtn").each(function () {
            //        if ($(this).find("p").attr("nssvalue") == data.securityId) {
            //            $(this).find("input").prop("checked", true).siblings("label").addClass("checked");
            //            $(this).addClass("liHover");
            //            isvalue = true;
            //            return false;
            //        }
            //    });
            //    if (!isvalue) {
            //        $("#supervisionPointInfo2 .radioBtn").each(function () {
            //            if ($(this).find("p").attr("nssvalue") == data.securityId) {
            //                $(this).find("input").prop("checked", true).siblings("label").addClass("checked");
            //                $(this).addClass("liHover");
            //                return false;
            //            }
            //        });
            //    }
            //} else {
            //    $("#supervisionPointInfo2 .radioBtn").each(function() {
            //        if ($(this).find("p").attr("nssvalue") == data.securityId) {
            //            $(this).find("input").prop("checked", true).siblings("label").addClass("checked");
            //            $(this).addClass("liHover");
            //            return false;
            //        }
            //    });
            //}           
            //$("#supervisionPointDetail").removeClass("hide");
            //$("#hid_securityId").val(data.securityId);
            //mapMonitor.showSupervisionPoint(data.securityId, data.name);
            //mapMonitor.rightListShow();


            that.setMarker(data);//影响消息框2次点击
            that.showInfoWindow(data);
            that.options.setMarkCallback(data);
            return this
        });
        //添加到地图
        map.addOverlay(marker);
    },
    setMarker: function (data) {
        var that = this;
        var options = this.options;
        var map = options.map;
        var data2 = options.securitySpotData;

        map.clearOverlays();

        for (var j = data2.length - 1; j >= 0; j--) {//循环添加
            if (data2[j] != data) {
                that.addMarker(data2[j], j);
            }
        }
        var arraypoint = this.options.arraypoint;
        if (arraypoint != null) {
            map.addOverlay(new BMap.Polyline(arraypoint, { strokeColor: "#ff3737", strokeWeight: 4, strokeOpacity: 1 }));   //增加折线
        }
        var number = data.securityMachine;
        var img = data.securityImg;      
        img = img.split('.')[0] + "_hover" + ".png";
        var image = new Image();
        image.src = img;
        var a = 32;
        var b = 47;
        var icon = new BMap.Icon(img, new BMap.Size(a, b), {
            anchor: new BMap.Size(12, 32)
        });
        var myClickIcon = icon;
        var myIcon = icon;
        //生成标签 设置标签位置
        var label;
        if (data.label.toString().indexOf('+')>0) {
            label = new BMap.Label(data.label, {
                offset: new BMap.Size(2, 6)
            });
        }
        else {
            if (parseInt(data.label) > 9) {
                if (parseInt(data.label) > 99) {
                    data.label = "99+";
                    label = new BMap.Label(data.label, {
                        offset: new BMap.Size(2, 6)
                    });
                }
                else {
                    //data.label = data.label;
                    label = new BMap.Label(data.label, {
                        offset: new BMap.Size(6, 6)
                    });
                }
            }
            else {
                label = new BMap.Label(data.label, {
                    offset: new BMap.Size(10, 6)
                });
            }
        }
        label.setStyle({
            background: 'none', color: '#fff', border: 'none'
        });
        //生成图标坐标
        var point = new BMap.Point(data.lng, data.lat);
        //添加坐标点属性,标签 注册回调事件
        var marker = new BMap.Marker(point, { icon: myIcon });
        marker.setTop(true);
        marker.setLabel(label);
        marker.number = number;//添加样式标识
        marker.myIcon = myIcon;//添加样式Url
        marker.myClickIcon = myClickIcon;//添加点击样式Url

        var p = marker.getPosition();//获取marker的位置
        that.mapSearch(p.lng, p.lat);//搜索并移动到marker的位置

        // alert(number);
        //alert(myClickIcon);
        //marker.addEventListener("click", function () {
        //    //event.stopPropagation();
        //    var p = marker.getPosition();//获取marker的位置
        //    //console.log("marker的位置是" + p.lng + "," + p.lat);
        //    that.mapSearch(p.lng, p.lat);//搜索并移动到marker的位置

        //    //$("#supervisionPointInfo2 input").prop("c.show();hecked", false);
        //    //$("#supervisionPointInfo2 label").removeClass("checked");
        //    //$("#supervisionPointInfo2 .radioBtn").removeClass("liHover");
        //    //$(this).find("input").prop("checked", true).siblings("label").addClass("checked");
        //    //$(this).addClass("liHover");
        //    //$("#supervisionPointDetail").removeClass("hide");
        //    //$("#hid_securityId").val(data.securityId);
        //    //mapMonitor.showSupervisionPoint(data.securityId);
        //    //mapMonitor.rightListShow();
        //    return this
        //});
        //添加到地图
        map.addOverlay(marker);
    },
    setMarkerEx: function (data) {
        var that = this;
        var options = this.options;
        var map = options.map;
        var data2 = options.securitySpotData;

        map.clearOverlays();
        var selno = 0;
        for (var j = data2.length - 1; j >= 0; j--) {//循环添加
            if (data2[j].sId != data.sId) {
                that.addMarker(data2[j], j);
            }
            else {
                //that.addMarker(data, j);
                selno = j;
            }
        }
        that.addMarker(data, j);
        var arraypoint = this.options.arraypoint;
        if (arraypoint != null) {
            //map.addOverlay(new BMap.Polyline(arraypoint, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 }));   //增加折线
            that.addOverlay(arraypoint);
        }
        that.mapSearch(data.lng, data.lat);//搜索并移动到marker的位置
        that.showInfoWindow(data);
        //添加到地图
        //map.addOverlay(marker);
    },
    //查找安检点
    mapSearch: function (par1, par2) {
        var options = this.options;
        var map = options.map;
        if ($.type(par1) == 'number' && $.type(par2) == 'number') {
            map.panTo(new BMap.Point(par1, par2));
        } else if ($.type(par1) == 'string' && $.type(par2) == 'string') {
            var myKeys = [par1, par2];
            var local1 = new BMap.LocalSearch(map, {
                renderOptions: { map: map, panel: "r-result" },
                pageCapacity: 5
            });
            local1.searchInBounds(myKeys, map.getBounds());
        } else if ($.type(par1) == 'string' || $.type(par2) == 'string') {
            var local2 = new BMap.LocalSearch(map, {
                renderOptions: { map: map }
            });
            local2.search(par1);
            local2.Bd != 0 && console.log('没有搜索结果');
        }
    },
    //查找安检点
    mapPanTo: function (par1, par2) {
        var options = this.options;
        var map = options.map;
        map.panTo(new BMap.Point(par1, par2));
    },
    //弹出信息窗口
    addInfoWindow: function (infoOptions) { 
        this.infoWindowOptions = infoOptions;
        //var options = this.options;
        //var map = options.map;
        //infoOptions = $.extend({
        //    title: '标题(可不传)',
        //    content: '内容',
        //    //width: 0,
        //    //height: 0,
        //    //maxWidth: 500,
        //    offset: new BMap.Size(-5, -20),
        //    enableAutoPan: true,
        //    enableCloseOnClick: true,
        //    imgClass: null,
        //    onOpen: function () {
        //        //alert('打开时的回调')
        //    },
        //    onClose: function () {
        //        //alert('关闭后的回调')
        //    },
        //    clickClose: function () {
        //        //alert('点击关闭按钮时')
        //    }
        //}, infoOptions);
        ////创建信息窗口对象
        //var infoWindow = new BMap.InfoWindow(infoOptions.content, infoOptions);  // 创建信息窗口对象
        //var p = new BMap.Point(infoOptions.lng, infoOptions.lat);
        //map.openInfoWindow(infoWindow, p);
        //function searchOverlay(on) { //检索图标并改变状态
        //    var allOverlay = map.getOverlays();
        //    for (var i = 0; i < allOverlay.length; i++) {
        //        var mark = allOverlay[i];
        //        var h = mark.getPosition();
        //        if (p.lat == h.lat && p.lng == h.lng) {
        //            console.log(this);
        //            mark.setIcon(on ? mark.myClickIcon : mark.myIcon);
        //        }
        //    }
        //}

        //          infoWindow.addEventListener('open', function () {
        //              if (infoOptions.imgClass) {
        //                  $('.'+infoOptions.imgClass)[0].onload = function () {
        //                      infoWindow.redraw();   //防止在网速较慢，图片未加载时，生成的信息框高度比图片的总高度小，导致图片部分被隐藏
        //                  };
        //              }
        //              searchOverlay(true);
        //              infoOptions.onOpen();
        //
        //          });
        //          infoWindow.addEventListener('close', function () {
        //              //console.log(2);
        //              searchOverlay(false);
        //              infoOptions.onClose();
        //
        //          });
        //          infoWindow.addEventListener('clickclose', function () {
        //              infoOptions.clickClose()
        //          })
    },
    //showInfoWindow: function (data) {
    //    
    //    var infoOptions = this.infoWindowOptions;
    //    if (!infoOptions)
    //    {
    //        return;
    //    }
    //    var options = this.options;
    //    var map = options.map;
       

    //    var point = new BMap.Point(data.lng, data.lat);
    //    var marker = new BMap.Marker(point); 

    //    var infoWindow = new BMap.InfoWindow(infoOptions.content, infoOptions);  // 创建信息窗口对象
    //    //var p = new BMap.Point(infoOptions.lng, infoOptions.lat);
    //    map.openInfoWindow(infoWindow, point);
    //},
    //弹出窗口
    showInfoWindow: function (data) {
        var infoOptions = this.infoWindowOptions;
        if (!infoOptions)
        {
            return;
        }
        var infoOptions1;
        for(var i=0;i<infoOptions.length;i++) {
            if(data.sId==infoOptions[i].id){
                infoOptions1 = infoOptions[i].content;
                break;
            }
        }
        var options = this.options;
        var map = options.map;


        var point = new BMap.Point(data.lng, data.lat);
        var marker = new BMap.Marker(point);
        infoOptions1.offset = new BMap.Size(0,-26);

        var infoWindow = new BMap.InfoWindow(infoOptions1.content, infoOptions1);  // 创建信息窗口对象
        map.openInfoWindow(infoWindow, point);

    },
    //渲染安检点
    renderSecuritySpotEx: function (data1) {
        var that = this;
        this.options.securitySpotData = data1;
        var options = this.options;
        var map = options.map;
        map.clearOverlays();
        var data = options.securitySpotData;
        for (var i = data.length - 1; i >= 0; i--) {//循环添加
            that.addMarker(data[i], i);
        }
    },
    addOverlay: function (arraypoint) {
        var options = this.options;
        this.options.arraypoint = arraypoint;
        var map = options.map;
        map.addOverlay(new BMap.Polyline(arraypoint, { strokeColor: "#ff3737", strokeWeight: 4, strokeOpacity: 1 }));   //增加折线
    }
};
$.extend({
    supervisionMap: function (options) {
        return $.extend({}, mapObj).init(options);
    }
});
