/**
 * Created by jackie on 2017/3/6.
 */
/*
 * 依赖于jquery
 *
 *调用方法
 * $.dialog({
 *   dialogType: 'warn' ,//alert info warn message 有四种快捷弹窗格式可选(默认模块全部显示,可不传)
 *   title: '提示', //提示信息的标题(默认为'提示',可不传)
 *   box: '容器名', //在那个容器中弹出(默认为'body',可不传)
 *   content: 提示信息内容，可以html对象，也可以是html文件(调用时需要ajaxGET请求文件名) 必须传；
 *   width: 300, //弹出框的宽度,具体到像素,不用加单位,(默认为300px,可不传)
 *   height: auto, //弹出框的高度,具体到像素,不用加单位,(默认为自适应高度,可不传,如果高度大于当前可视窗口高度,那么让弹出框高度为当前(窗口高度-20),溢出出滚动条)
 *   top: 20, //弹出框拒顶部的高,百分比数值,不用加单位(默认50%居中,可不传)
 *   left: 50, //弹出框拒左侧的距离 百分比数值,不用加单位(默认50%居中,可不传)
 *   icon: null, //ask ok warn 有三种快窗格内标识(默认无图标,可不传)
 *   closeBtn: true,//右上角关闭按钮(默认存在,可不传)
 *   beforeOk: 确定前的回调(默认无,可不传)
 *   onOk: 点确定的回调(默认无,可不传)
 *   onCancel: 点取消或关闭的回调(默认无,可不传)
 *   btnList: 底部按钮的参数(默认为'确定/取消'),可传字符串,字符串数组,对象数组,字符串/对象混合数组
 *   style3d:0,//css3效果,参数为数字(0-5)(默认无效果,可不传)[无效果/From Below/Sticky Up/3D Flip(horizontal)/3D Flip(vertical)/3D Sign]
 * })
 *
 *   btnList例:(说明: 当按钮组为三个或者三个以上时,按钮样式为确定按钮的默认样式)
 *        *  btnList:'none', //为none时  按钮组不显示
 *           btnList:'保存', //除了为none时的单独字符串,都会改变'确定'按钮的名称
 *           btnList:['aa','bb'],  //底部'确定/取消'按钮内容改为'aa','bb'
 *           btnList:['aa','bb','cc'], //底部按钮为三个,内容改为'aa','bb','cc';
 *           btnList: [{name: 'aa', callback: function (){}}, {name: 'bb', callback: function () {}}], //底部按钮内容为'aa','bb',并且点击的时候会有相应的callback执行(与onOK/onCancel/beforeOK不冲突,可同时启用)
 *           btnList: ['aa', {name: 'bb', callback: function () {}}], //底部按钮内容为'aa','bb',点击'bb'的时候会有相应的callback执行
 *           btnList: [{name:'aa'}, {name: 'bb', callback: function () {}}], //底部按钮内容为'aa','bb',点击'bb'的时候会有相应的callback执行
 * */

(function($){
    var dialogObj = {
        //初始化参数
        init: function (options) {
            options = $.extend({
                box: null,
                dialogType: null,
                title: "提示",
                content: '这里是内容',
                icon: null,
                //width: 300,
                //height:　'auto',
                //top: 50,
                //left: 50,
                style3d:0,//[无效果/From Below/Sticky Up/3D Flip(horizontal)/3D Flip(vertical)/3D Sign]
                closeBtn: true,
                btnList: null,
                beforeOk: null,
                onOk: null,
                onCancel: null
            }, options);
            this.config = options;
            var that = this;
            /*//加载样式
             var href = $("script[src$='dialog/dialog.js']").attr("src").replace("dialog.js", "dialog.css");
             if ($("link[href='" + href + "']").length == 0) {
             $("head").append("<link href='" + href + "' rel='stylesheet'/>");
             }*/
            that.initDom();
            that.initEvents();
            that.moveDialog();
            return that;
        },
        //加载DOM结构
        initDom: function () {
            var that = this;
            var config = this.config;
            //创建文档碎片
            var container = $("<div class='plugin_dialog'><div class='plugin_dialog_bg'></div><div class='cssStyle'><div class='plugin_dialog_content'><div class='plugin_dialog_hd'><div class='plugin_dialog_logotop'><div class='title_box'><p id='title'></p></div></div><a href='javascript:' class='closeBtn close' title='关闭'></a></div><div class='content plugin_dialog_bd'></div><div class='standard Btn'><button class='send' type='submit'>确定</button><button class='cancelBtn send2 close' type='button'>取消</button></div></div></div></div>");
            //右上角关闭按钮是否存在
            !config.closeBtn && container.find(".closeBtn").hide();
            //是否调用简单样式
            switch (config.dialogType) {
                case "alert":
                    //弹出框 隐藏取消按钮
                    container.find(".send2").hide();
                    break;
                case "info":
                    //信息框 隐藏标题和按钮
                    container.find(".plugin_dialog_hd,.Btn").hide();
                    //自动关闭
                    setTimeout(function () {
                        that.container.remove();
                        that.config.onOk && that.config.onOk();
                    }, 3000);
                    break;
                case "warn":
                    //警告提示框 设置标题样式
                    container.addClass("plugin_dialog_warning");
                    //隐藏关闭按钮、遮罩层
                    container.find(".plugin_dialog_bg,.send2").hide();
                    break;
                case "message":
                    //信息框 隐藏标题和按钮
                    container.find(".plugin_dialog_hd,.Btn").hide();
                    break;
            }
            //拆分按钮对象属性
            if (config.btnList) {
                var $send = container.find(".Btn .send");//默认确定按钮1
                var $send2 = container.find(".Btn .send2");//默认取消按钮2
                if($.type(config.btnList) == 'string'){
                    $send.text(config.btnList);//值为其他字符串类型数据
                    config.btnList == 'none' && container.find(".Btn").hide();//值为字符串'none'则隐藏按钮
                }
                if ($.type(config.btnList) == 'array') {
                    switch (config.btnList.length) {
                        case 1:
                            $.type(config.btnList[0]) == 'object' ? $send.text(config.btnList[0].name).click(config.btnList[0].callback) : $send.text(config.btnList[0]);
                            container.find(".Btn .send2").remove();
                            break;
                        case 2:
                            $.type(config.btnList[0]) == 'object' ? $send.text(config.btnList[0].name).click(config.btnList[0].callback) : $send.text(config.btnList[0]);
                            $.type(config.btnList[1]) == 'object' ? $send2.text(config.btnList[1].name).click(config.btnList[1].callback) : $send2.text(config.btnList[1]);
                            break;
                        default :
                            var $btnList = container.find(".Btn").html('');
                            for (var i = 0; i < config.btnList.length; i++) {
                                $.type(config.btnList[i]) == 'object' ? $btnList.append($("<button class='send' type='submit'>" + config.btnList[i].name + "</button>").click(config.btnList[i].callback)) : $btnList.append($("<button class='send' type='submit'>" + config.btnList[i] + "</button>"));
                            }
                    }
                }
            }
            //设置title内容
            container.find("#title").text(that.config.title);
            //如果传入类型是String类型，则加入plugin_dialog_m40样式
            if ($.type(that.config.content) != "string") {
                var content = that.config.content;
            } else {
                content = "<i class='" + that.config.icon + "'></i>" + that.config.content;
                container.find(".content").addClass("plugin_dialog_m40");
            }
            container.find(".content").append(content);
            container.find(".plugin_dialog_content").css('display','none');//先设置元素为不显示
            if(that.config.box){
                var box = that.config.box;
                var $boxCur = $(box);
                container.find(".plugin_dialog_bg").css({//设置阴影大小和位置
                    top:$boxCur.offset().top,
                    left:$boxCur.offset().left,
                    width:$boxCur.width(),
                    height:$boxCur.height()
                });
                //获取参数并设定完毕后写入dom
                $boxCur.append(container);
            }else{
                $('body').append(container);
            }
            //先执行确定或取消前的回调
            config.beforeOk && config.beforeOk();

            //设置宽高和默认上下左右居中
            var $doc = $('.plugin_dialog_content'); //获取元素暂存
            this.config.dialogType != "warn" && $doc.css({ //当弹出框类型不为warn时 设置宽高和位置
                width: config.width || 300,
                height: config.height || 'auto',
                left: (config.left || 50) + '%',
                top: (config.top || 50) + '%',
                overflow : 'auto'
            });
            parseInt($doc.css('height')) >= $(window).height() && $doc.css('height', $(window).height()-20); //如果高度大于当前可视窗口高度,那么让弹出框高度为当前(窗口高度-20)
            $doc.css('left') == '50%' && $doc.css('marginLeft', -($doc.width() / 2));//水平居中
            $doc.css('top') == '50%' && $doc.css('marginTop', -($doc.height() / 2));//垂直居中
            $doc.css('display', 'block');//显示设置好宽高和位置偏移的元素

            var style3d = that.config.style3d;
            var styles = ['','scale','translate','horizontal','vertical','Sign'];
            $doc.addClass(styles[style3d]);
            window.setTimeout(function () {
                $('.plugin_dialog').addClass('show')
            },100);
            that.container = container;
        },
        //初始化事件
        initEvents: function () {
            var that = this;
            //关闭、取消
            that.container.find(".close").click(function () {
                that.container.remove();
                that.config.onCancel && that.config.onCancel()
            });
            //确定
            that.container.find(".send").click(function () {
                that.container.remove();
                that.config.onOk && that.config.onOk();
            });
        },
        //对话框标题鼠标按下时开启移动模式，抬起时关闭移动模式
        moveDialog: function () {
            var that = this;
            var isMoving = false, position = {};
            that.container.find(".plugin_dialog_hd").mousedown(function (e) {
                //记住本次对话框的位置，并标志可以移动对话框
                isMoving = true;
                position = {x: e.clientX, y: e.clientY};
                var contentDom = that.container.find(".plugin_dialog_content");
                //警告提示框需要消除bottom，并设置top
                if (that.config.dialogType == "warn") {
                    contentDom.css("top", contentDom.position().top);
                    contentDom.css("bottom", "auto");
                }
            }).mouseup(function () {
                isMoving = false;
                position = {};
            });
            //对话框移动
            $(document).mousemove(function (e) {
                if (!isMoving) return;
                //根据鼠标移动情况设置对话框位置
                var contentDom = that.container.find(".plugin_dialog_content");
                var currentPosition = contentDom.position();
                var top = currentPosition.top + (e.clientY - position.y);
                var left = currentPosition.left + (e.clientX - position.x);
                contentDom.css({top: top + "px", left: left + "px"});
                //记住本次对话框的位置
                position = {x: e.clientX, y: e.clientY};
            });
        }
    };
    $.extend({
        dialog: function(options) {
            return $.extend({}, dialogObj).init(options);
        }
    });
})(jQuery);