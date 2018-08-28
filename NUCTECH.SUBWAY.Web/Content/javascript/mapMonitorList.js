; (function ($) {

    var mapMon = function () {
        var thiz = this;
        thiz.i = 0;
        thiz.pageIndex = 1;
        thiz.pageIndex2 = 1;
        thiz.pageIndex3 = 1;
         
    }

    mapMon.prototype = {
        init: function () {
            if (typeof (BMap) == "undefined") alert("没有联网！地图无法加载。");
            var thiz = this;
            
            $('#searchBtn').click(function () {
                if ($('#alllist').hasClass('active')) {
                    thiz.getLeftList();
                }
                 
            });
            thiz.initMap();
            $('.divdownbtn').click(function () {
                $('.divdownbtn').hide();
                $('.divupbtn').show();
                $('.mapmain .mapleft').animate({ height: "9.5%" });
                $('.mapleftlist').slideUp();
            });
            $('.divupbtn').click(function () {
                $('.divdownbtn').show();
                $('.divupbtn').hide();
                $('.mapmain .mapleft').animate({ height: "43.5rem" });
                $('.mapleftlist').slideDown();
            });
            $('#alllist').click(function () {
                $('#searchTxt').val('');
               
                $(this).addClass('active');
                $('.mapleftlist_right').fadeOut().animate({ left: "100%" });
                $('.mapleftlist_left').animate({ left: "0" }).fadeIn(300);
                thiz.pageIndex = 1;
                thiz.getLeftList();
            });
            
            $('.colsetittle').click(function () {
                $('.topmessage').slideToggle();
                $('.mapmain .mapleft').animate({ top: "0" });
                $('.mapmain .mapright').animate({ top: "0" });
            });
            thiz.citySelectEvt();

            if ($('#hid_securityName').val() != '') {
                thiz.mapLeftClick('.mapleftbtnlist1 .mapleftfont');
            }
            else {
                thiz.initPage();
                thiz.collectSecurity();
                thiz.allListClick();
               
            }


        },
        citySelectEvt: function () {
            var thiz = this;
            $('#sel_cities').change(function () {
                thiz.initMap();
                thiz.getLeftList(); 
            });

        },
        initPage: function () {
            var thiz = this;
            var total = parseInt($('#pageTotal').val());
            if (total <= 0) {
                return;
            }
            $("#pages").createPage({
                totalCount: parseInt($('#pageTotal').val()),
                pageSize: parseInt($('#pageTotal').attr('pageSize')),
                current: thiz.pageIndex,
                type: 'simple',
                pageSizeShow: 'show',
                backFn: function (p) {
                    thiz.pageIndex = p;
                    thiz.getLeftList();
                }
            });
        },
        initPage3: function () {
            var thiz = this;
            var total = parseInt($('#pageTotal3').val());
            if (total <= 0) {
                return;
            }
            $("#pages3").createPage({
                totalCount: parseInt($('#pageTotal3').val()),
                pageSize: parseInt($('#pageTotal3').attr('pageSize')),
                current: thiz.pageIndex3,
                type: 'simple',
                pageSizeShow: 'show',
                backFn: function (p) {
                    thiz.pageIndex3 = p; 
                }
            });
        },
        getLeftList: function () {
            var thiz = this;

            $('.mapright').hide();
            var paras = {
                cityCode:   $('#sel_cities').val() ,
                countryCode:   $('#sel_cities').val(),
                securityName: $('#searchTxt').val(),
                pageIndex: thiz.pageIndex,
                pageSize: $('#pageTotal').attr('pageSize')

            };


            $.ajax({
                url: "GetLeftPartial",
                type: "POST",
                dataType: "html",
                data: paras,
                success: function (result) {
                    $('.mapleftlist_left').empty().html(result);

                    thiz.initPage();
                    thiz.initMap();
                    thiz.allListClick();
                    thiz.collectSecurity();
                },
                error: function (er) {

                }
            });
        },
         
        collectSecurity: function () {
            var thiz = this;
            var isCanClick = true;
            $('.mapleftlist ul li .right a').click(function () {
                if (!isCanClick) {
                    return;
                }
                isCanClick = false;

                var _thiz = $(this);

                var obj = _thiz.find('img');
                var tp = 1;
                var prompt = '收藏成功';
                if (obj.attr('src').toLowerCase() == "/content/images/mapstarnull.png") {
                    obj.attr('src', '/content/images/mapstarall.png');
                }
                else {
                    obj.attr('src', '/content/images/mapstarnull.png');
                    tp = 0;
                    prompt = '取消收藏';
                }

                $.ajax({
                    url: 'SaveUserCollect',
                    type: 'post',
                    data: { securityId: _thiz.attr('securityid'), collType: tp },
                    success: function (result) {
                        if (result.Status) {
                            var ePop = _thiz.parent().find('.pop');
                            var pop = $('<div class="pop" style="opacity:0; display: none;">' + prompt + '</div>');
                            if (ePop.length != 0) {
                                ePop.remove();
                            }
                            _thiz.parent().append(pop);
                            ePop = pop;
                            //_thiz.removeClass(cn);
                            ePop.css('display', 'block').animate({ 'opacity': '1' });
                            ePop.animate({ 'opacity': '0' }, 3000, function () {

                                $(this).css('display', 'none').text(prompt);
                               
                                isCanClick = true;
                            }); 
                        }
                    }
                });
            });
        },
        initMap: function () {
            var thiz = this;

            var lnglats = $('.mapleftbtnlist1 .mapleftfont');
            var datas = new Array();
            //var datas = [
            //    { "lng": 116.39782, "lat": 39.913561, 'label': '99', "securityImg": "/Content/Images/mapcolseall.png" },
            //    { "lng": 116.404503, "lat": 39.926588, 'label': '0', "securityImg": "/Content/Images/mapcolseall.png" },
            //    { "lng": 116.404503, "lat": 39.936588, 'label': '10', "securityImg": "/Content/Images/mapopenall.png" },
            //    { "lng": 116.404503, "lat": 39.900588, 'label': '8', "securityImg": "/Content/Images/mapopenall.png" },
            //    { "lng": 116.404503, "lat": 39.909588, 'label': '60', "securityImg": "/Content/Images/mapopenpart.png" }
            //]


            $.each(lnglats, function (index, el) {

                var lnglat = $(el).attr('lngandlat').split(',');
                var edCount = parseInt($(el).attr('enabledevicecount'));
                var opCount = parseInt($(el).attr('opendevicecount'));
                var imtUrl = '/Content/Images/mapcolseall.png';
                var securityId = $(el).attr('securityid');
                if (edCount > 0) {
                    if (opCount < edCount) {
                        imtUrl = '/Content/Images/mapopenpart.png';
                    }
                    else if (opCount == edCount) {
                        imtUrl = '/Content/Images/mapopenall.png';
                    }
                }

                var tmp = { sId: securityId, "lng": parseFloat(lnglat[0]), "lat": parseFloat(lnglat[1]), 'label': edCount, "securityImg": imtUrl };

                datas.push(tmp);

            })
            if (typeof (BMap) == "undefined") return;
            thiz.mapBaidu = $.supervisionMap({
                defaultCity: '北京市',
                isOnLine: $('#isOnLine').val(),         //1:在线;0:离线
                mapTilesUrl: $('#mapTilesUrl').val(),   //空值表示默认路径
                setMapZoom: 12,
                mapContainerId: 'mapWrap',
                securitySpotData: datas,
                setMarkCallback: function (data) {

                    if (data) {
                        $('.mapleftbtnlist1 li').removeClass('liactive');

                        $.each($('.mapleftbtnlist1 li'), function (index, el) {
                            var sid = $(this).find('.mapleftfont').attr('securityid');
                            if (data.sId == sid) {
                                $(this).addClass('liactive');
                                thiz.getRightList(sid, '');
                            }
                        });
                    }
                }
            });


        },
        allListClick: function () {
            var thiz = this;
            $('.mapleftbtnlist1 .mapleftfont').click(function () {
                thiz.mapLeftClick(this);
            });
        },
        mapLeftClick: function (obj) {

            var thiz = this;
            $('#colsedialog2').click();
            $('#colsedialog').click();
            $('#colsedialogList').click();

            $('.mapleftbtnlist1 li').removeClass('liactive');
            $(obj).parent().addClass('liactive');

            var lnglat = $(obj).attr('lngandlat').split(',');
            var edCount = parseInt($(obj).attr('enabledevicecount'));
            var opCount = parseInt($(obj).attr('opendevicecount'));
            var imtUrl = '/Content/Images/mapcolseall.png';

            if (edCount > 0) {
                if (opCount < edCount) {
                    imtUrl = '/Content/Images/mapopenpart.png';
                }
                else if (opCount == edCount) {
                    imtUrl = '/Content/Images/mapopenall.png';
                }
            }

            var datas = { sId: $(obj).attr('securityid'), "lng": parseFloat(lnglat[0]), "lat": parseFloat(lnglat[1]), 'label': edCount, "securityImg": imtUrl };

            if (typeof (BMap) != "undefined") {
                thiz.mapBaidu.setMarker(datas);
            }
            thiz.pageIndex2 = 1;
            thiz.getRightList($(obj).attr('securityid'), '');

        },
        userCollectClick: function () {
            var thiz = this;
            $('.mapleftbtnlist2 .mapleftfont').click(function () {

                $('.mapleftbtnlist2 li').removeClass('liactive');
                $(this).parent().addClass('liactive');

                var lnglat = $(this).attr('lngandlat').split(',');
                var edCount = parseInt($(this).attr('enabledevicecount'));
                var opCount = parseInt($(this).attr('opendevicecount'));
                var imtUrl = '/Content/Images/mapcolseall.png';

                if (edCount > 0) {
                    if (opCount < edCount) {
                        imtUrl = '/Content/Images/mapopenpart.png';
                    }
                    else if (opCount == edCount) {
                        imtUrl = '/Content/Images/mapopenall.png';
                    }
                }

                var datas = { "lng": parseFloat(lnglat[0]), "lat": parseFloat(lnglat[1]), 'label': edCount, "securityImg": imtUrl };

                if (typeof (BMap) != "undefined") {
                    thiz.mapBaidu.setMarker(datas);
                }
                thiz.pageIndex2 = 1;
                thiz.getRightList($(this).attr('securityid'), '');
            });
        },
        rightEvent: function () {
            var thiz = this;

            $('#colseright').click(function () {
                $('.mapleftbtnlist1 li').removeClass('liactive');
                $('.mapright').hide();

            });
            $('#maprightcolse').click(function () {
                if (thiz.i == 0) {
                    $(this).removeClass('maprightshow2');
                    $(this).addClass('maprightshow2hover');
                    thiz.i = 1;
                }
                else {
                    $(this).removeClass('maprightshow2hover');
                    $(this).addClass('maprightshow2');
                    thiz.i = 0;
                }
                $('.maprightlist').slideToggle();
            });
            $('#colsedialog').click(function () {
                $(this).parent().parent().parent().parent().hide();
                if ($('.mapvideo2').length > 0) {
                    $('.mapvideo2').removeClass('mapvideo2hover');
                } else {
                    $('.maprightlist ul li a').removeClass('mapvideohover');
                }

            });
            $('#colsedialog2').click(function () {
                $(this).parent().parent().parent().parent().hide();
                $('.maprightlist ul li a').removeClass('mappichover');

                clearInterval(thiz.getImgInterval);
            });
            $('#colsedialogList').click(function () {
                $(this).parent().parent().parent().parent().hide();
                $('.maprightlist ul li a').removeClass('mapvideohover');
                $('.videoList ul').empty();

            });

            $('.maprightlist .mapvideo').unbind('click').click(function () {
                $('.maprightlist ul li a').removeClass('mapvideohover');
                $(this).addClass('mapvideohover');
                var videoNums = $(this).attr('deviceNums');
                if (videoNums > 1) {

                    thiz.videoPageIndex = 1;
                    thiz.getVideoList(thiz.videoPageIndex, $(this).attr('deviceid'), '');
                    $('#dialogvideo2 .left').text(thiz.subStringExtend($(this).attr('devicename'), 30) + '-视频监控');
                    $('#dialogvideo2 .left').attr('title', $(this).attr('devicename'));
                    $('#dialogvideo2 #hid_deviceNums').val($(this).attr('devicenums'));
                    $('#dialogvideo2 #hid_deviceId').val($(this).attr('deviceid'));

                    $('#dialogvideo2').show();
                    thiz.videoDrag('dialogvideo2');

                    $('.videoListNext').unbind('click').click(function () {

                        var tmp = thiz.videoPageIndex + 1;
                        if (tmp > $('.pointPage ul li').length) {
                            tmp = $('.pointPage ul li').length;
                        }
                        else {
                            thiz.getVideoList(tmp, $('#hid_deviceId').val(), 'right');
                        }
                        thiz.videoPageIndex = tmp;


                    });
                    $('.videoListPrev').unbind('click').click(function () {
                        var tmp = thiz.videoPageIndex - 1;
                        if (tmp < 1) {
                            tmp == 1;
                        }
                        else {
                            thiz.getVideoList(tmp, $('#hid_deviceId').val(), 'left');
                        }
                        thiz.videoPageIndex = tmp;


                    });




                } else {
                    $('#dialogvideo .left').text(thiz.subStringExtend($(this).attr('devicename'), 30) + '-视频监控');
                    $('#dialogvideo .left').attr('title', $(this).attr('devicename'));
                    $('#dialogvideo').show();
                    thiz.videoDrag('dialogvideo');

                    var item = {}
                    var vip = $(this).attr("videoip");
                    var cIpAndPort;
                    if (vip != null && vip.indexOf('|') > 0) {
                        cIpAndPort = $(this).attr("videoip").split('|');
                        item.ip = cIpAndPort[0];
                        item.port = cIpAndPort[1];
                    }

                    item.username = $(this).attr("username") != null ? $(this).attr("username") : "";
                    item.password = $(this).attr("userpwd") != null ? $(this).attr("userpwd") : "";

                    thiz.initVideo(item);
                }

            });
            $('.maprightlist .mappic').unbind('click').click(function () {
                $('.maprightlist ul li a').removeClass('mappichover');
                $(this).addClass('mappichover');
                $('#dialogpic .left').text(thiz.subStringExtend($(this).attr('devicename'), 30) + '-图像监控');
                $('#dialogpic .left').attr('title', $(this).attr('devicename'));
                $('#dialogpic').show();
                thiz.videoDrag('dialogpic')

                var dId = $(this).attr('deviceid');
                thiz.imgInterval = $('#hid_interval').val() == '' ? 3000 : parseInt($('#hid_interval').val());

                thiz.getImgInterval = setInterval(function () {
                    $.ajax({
                        url: 'GetPackageImg',
                        type: 'post',
                        data: { deviceId: dId },
                        dataType: 'json',
                        success: function (data) {
                            if (data) {
                                var oldUrl = $('#dialogpic').find('img').attr('src');
                                if (oldUrl == data) {
                                    return;
                                }
                                if (data != oldUrl) {
                                    $('#dialogpic').find('img').attr('src', data);
                                    $('#dialogpic').find('img').error(function () {
                                        $(this).attr('src', '../../Content/Images/noimg.png').width('229px').height('125px').css('margin-top','15%'); 
                                    });
                                }
                            }
                            else {
                                $('#dialogpic').find('img').attr('src', '/Content/Images/noimg.png').width('229px').height('125px').css('margin-top', '15%'); 
                            }
                        }

                    });

                }, thiz.imgInterval);

            });
            thiz.enterpriceChangeEvt();


        },
        getVideoList: function (pageIndex, dId, direction) {
            var thiz = this;

            $.ajax({
                url: 'GetVideoList',
                type: 'post',
                data: { deviceId: dId, pageIndex: pageIndex, pageSize: 4 },
                success: function (data) {
                    if (data.length > 0) {
                        var strHtm = '';
                        var strPageHtm = '';

                        $.each(data, function (index, el) {
                            strHtm += '<li><div><div title= ' + el.VideoName + '>' + thiz.subStringExtend(el.VideoName, 30) + '</div><div  style="position:relative"> <a href="#" class="mapvideo2" userip=' + el.VideoIp + ' username=' + el.VideoUserName + ' userpwd=' + el.VideoPassword + ' videoname=' + el.VideoName + '></a></div></div><div class="clear"></div></li>';
                        });
                         
                        var pageCount = Math.ceil($('#hid_deviceNums').val() / 4);
                        for (var i = 0; i < pageCount; i++) {

                            if (i == pageIndex-1) {
                                strPageHtm += '<li><a href="#" class="active" index=' + (i + 1) + '><img src= "../../../../Content/Images/yuan_liang.png" /></a></li>';
                            }
                            else {
                                strPageHtm += '<li><a href="#" index=' + (i + 1) + '><img src= "../../../../Content/Images/yuan.png" /></a></li>';
                            }
                        }
                         
                        if ($('.videoList ul li').length > 0) {
                            if (direction == 'right') {
                                $('.videoList').animate({ 'left': "-100%" }, 1000, function () {
                                    $('.videoList ul').empty().append(strHtm);
                                    $('.videoList').css('left', '0');
                                    thiz.videoListPlayVideo();
                                });
                            }
                            else {
                                $('.videoList').animate({ 'left': "100%" }, 1000, function () {
                                    $('.videoList ul').empty().append(strHtm);
                                    $('.videoList').css('left', '0');
                                    thiz.videoListPlayVideo();
                                });
                            }


                        }
                        else {
                            $('.videoList ul').empty().append(strHtm);
                        }


                        $('.pointPage ul').empty().append(strPageHtm);

                        if (pageCount > 1) {
                            $('.videoListNext').css('background', 'url("../../../../Content/Images/right.png") 30% 50% no-repeat');
                        }
                        else {
                            $('.videoListNext').css('background', 'url("../../../../Content/Images/right_no.png") 30% 50%  no-repeat');
                        }

                        thiz.videoPageClick();
                        thiz.videoListPlayVideo();
                        
                    }
                }
            });
        },
        videoListPlayVideo: function () {
            var thiz = this;

            $('.mapvideo2').unbind('click').click(function () {

                if ($('#dialogvideo').css('display') == 'block') {
                    return;
                }
                $(this).addClass('mapvideo2hover');

                $('#dialogvideo .left').text(thiz.subStringExtend($('#dialogvideo2 .left').attr('title'), 30) + '-' + thiz.subStringExtend($(this).attr('videoname'), 30) + '-视频监控');
                $('#dialogvideo .left').attr('title', $(this).attr('devicename'));
                $('#dialogvideo').show();
                thiz.videoDrag('dialogvideo')

                var item = {}
                var cIpAndPort = $(this).attr("userip").split('|');

                item.ip = cIpAndPort[0];
                item.port = cIpAndPort[1];
                item.username = $(this).attr("username");
                item.password = $(this).attr("userpwd");

                thiz.initVideo(item);

            });
        },
        videoPageClick: function () {
            var thiz = this;

            $('.pointPage ul li a').unbind('click').click(function () {
                $('.pointPage ul li a').removeClass('active');
                $('.pointPage ul li a').find('img').attr('src', '../../../../Content/Images/yuan.png');

                $(this).addClass('active');
                $(this).find('img').attr('src', '../../../../Content/Images/yuan_liang.png');
                var currIndex = $(this).attr('index');
                var direc = 'left';
                if (parseInt(currIndex) == thiz.videoPageIndex) {
                    return;
                }
                if (parseInt(currIndex) > thiz.videoPageIndex) {
                    direc = 'right';
                }
                thiz.videoPageIndex = parseInt(currIndex);
                thiz.getVideoList(currIndex, $('#hid_deviceId').val(), direc);

            });

        },
        initVideo: function (config) {
            var thiz = this;

            thiz.playVideo(config, $('#dialogvideo .mapdialogcontent'));

        },
        playVideo: function (config, dom) {

            var player = $.videoplayer($.extend({
                container: dom,
                beforeOpenSound: function () {

                }
            }, config));
            player.start();

        },
        enterpriceChangeEvt: function () {
            var thiz = this;
            $('.selectenterprice').unbind('change').change(function () {
                thiz.pageIndex2 = 1;
                thiz.getEnableDevices();
            });
        },
        subStringExtend: function (str, len) {
            var regexp = /[^\x00-\xff]/g;// 正在表达式匹配中文  
            // 当字符串字节长度小于指定的字节长度时  
            if (str.replace(regexp, "aa").length <= len) {
                return str;
            }
            // 假设指定长度内都是中文  
            var m = Math.floor(len / 2);
            for (var i = m, j = str.length; i < j; i++) {
                // 当截取字符串字节长度满足指定的字节长度  
                if (str.substring(0, i).replace(regexp, "aa").length >= len) {
                    return str.substring(0, i) + '...';
                }
            }
            return str;
        },
        getRightList: function (sId, eId) {

            var thiz = this;
            $.ajax({
                url: 'GetSecurityDetial',
                type: 'post',
                dataType: 'html',
                data: { securityId: sId, enterpriceId: eId },
                success: function (html) {

                    $('.mapright').show().empty().html(html);
                    thiz.initPage2();
                    thiz.rightEvent();
                    if ($('#pageTotal2').val() == "0") {
                        $('.simplepage2').remove();
                    }

                }
            });
        },
        initPage2: function () {
            var thiz = this;
            var total = parseInt($('#pageTotal2').val());
            if (total <= 0) {
                return;
            }
            $("#pages2").createPage({
                totalCount: parseInt($('#pageTotal2').val()),
                pageSize: parseInt($('#pageTotal2').attr('pageSize')),
                current: thiz.pageIndex2,
                type: 'simple',
                pageSizeShow: 'show',
                backFn: function (p) {
                    thiz.pageIndex2 = p;
                    thiz.getEnableDevices();
                }
            });
        },
        getEnableDevices: function () {
            var thiz = this;

            $.ajax({
                url: 'GetSecurityEnableDevice',
                type: 'post',
                dataType: 'html',
                data: {
                    securityId: $('#hid_sId').val(),
                    enterpriceId: $('.selectenterprice').val(),
                    pageIndex: thiz.pageIndex2,
                    pageSize: $('#pageTotal2').attr('pageSize')
                },
                success: function (data) {
                    $('#dCount').text($('#pageTotal2').val());
                    if (data) {
                        var objData = JSON.parse(data);
                        var strHtml = '';
                        $.each(objData, function (index, el) {

                            strHtml += '<li><div class="left" title= "' + el.DeviceName + '" >' + thiz.subStringExtend(el.DeviceName, 12) + '</div><div class="right" style="margin-left:2%;"><div class="heightcenter"><a href="#" class="mapvideo" deviceid="' + el.DeviceID + '" devicename="' + thiz.subStringExtend(el.DeviceName, 30) + '" "></a></div></div><div class="right"><div class="heightcenter"><a href="#" class="mappic" deviceid="' + el.DeviceID + '" devicename="' + thiz.subStringExtend(el.DeviceName, 30) + '"></a></div></div><div class="clear"></div></li >';

                        });
                        strHtml += '';

                        $('.maprightlist ul').children().remove();
                        $('.maprightlist ul').append(strHtml);

                        thiz.initPage2();
                        thiz.rightEvent();
                    }

                }
            });
        },
        videoDrag: function (id) {
            $('#' + id).dragging({ ifDrag: true, dragLimit: true });
        },
    }

    $(document).ready(function () {
        var mp = new mapMon();
        mp.init();

        //var chat = $.connection.mapMessageHub;
        //chat.client.addMapMessage = function (name, message) {
        //    var strhtm = '<li><div class="heightcenter"><a href="#">' + name + ':' + message + '</a></div></li>';

        //    $('#mqNotify>ul').append(strhtm);
        //};

        //$.connection.hub.start().done(function () {
        //    //chat.server.send('aaa','sss');
        //});
    });

})(jQuery)