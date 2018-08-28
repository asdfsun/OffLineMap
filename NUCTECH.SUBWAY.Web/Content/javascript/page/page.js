(function ($) {
    var pg = {
        init: function (obj, args) {
            return (function () {
                ////1.加载dom
                //var baseUrl = $("script[src$='page.js']").attr("src").replace("page.js", "");
                ////加载样式表
                //var styleUrl = baseUrl + "pageStyle.css";
                //if ($("link[href='" + styleUrl + "']").length < 1) {
                //    $("head").append("<link href='" + styleUrl + "' rel='stylesheet' type='text/css' />");
                //}

                pg.fillHtml(obj, args);
                pg.bindEvent(obj, args);
            })();
        },
        //渲染html
        fillHtml: function (obj, args) {
            return (function () {
                obj.empty();

                //首页
                if (args.type === "normal") {
                    if (args.current > 1) {
                        obj.append('<a href="javascript:;" class="firstPage"><<</a>');
                    } else {
                        obj.remove('.firstPage');
                        obj.append('<a href="javascript:;" class="disabled firstPage"><</a>');
                    }
                }
                //显示当前共多少条
                if (args.pageSizeShow == "show") {
                    obj.append('<sapn  class="pageSizeShow">共' + args.totalCount + '条</span>');
                }

                //上一页
                if (args.current > 1) {
                    obj.append('<a href="javascript:;" class="prevPage"><</a>');
                } else {
                    obj.remove('.prevPage');
                    obj.append('<a href="javascript:;" class="disabled disLeft"><</a>');
                }
                //复杂模式
                if (args.type === 'complex') {
                    //显示页码1 2           
                    if (args.current > 2 && args.current >= 4 && args.pageCount > 9) {
                        obj.append('<a href="javascript:;" class="tcdNumber">' + 1 + '</a>');
                        obj.append('<a href="javascript:;" class="tcdNumber">' + 2 + '</a>');
                    }
                    //左侧出现省略号
                    if (args.current > 5 && args.current <= args.pageCount && args.pageCount > 9) {
                        obj.append('<span class="ellipsis">...</span>');
                    }

                    //中间5个页码
                    var start = args.current - 2, end = args.current + 2;

                    //第一页
                    if (args.current == 1) {
                        end += 4;
                    }
                    //最后一页
                    if (args.current >= args.pageCount) {
                        start -= 4;
                    }

                    //头尾两个特殊处理
                    if (args.current == 4) {
                        start++;
                    }
                    if (args.current == (args.pageCount - 3) && args.pageCount > 9) {
                        end--;
                    }

                    //小于等于9页情况
                    if (args.pageCount <= 9) {
                        start = 1;
                        end = 9;
                    } else {//大于9页情况
                        if (args.current > 1 && args.current < 5) {
                            end += 5 - args.current;
                        }
                        if (args.current < args.pageCount && args.current > (args.pageCount - 4)) {
                            start -= (args.current - args.pageCount) + 4;
                        }
                    }
                    //生成页码
                    for (; start <= end; start++) {
                        if (start >= 1 && start <= args.pageCount) {
                            if (start != args.current) {
                                obj.append('<a href="javascript:;" class="tcdNumber">' + start + '</a>');
                            } else {
                                obj.append('<a href="javascript:;" class="current">' + start + '</a>');
                            }
                        }
                    }
                    //末尾省略号
                    if (args.current < args.pageCount - 4 && args.current >= 1 && args.pageCount > 9) {
                        obj.append('<span class="ellipsis">...</span>');
                    }
                    //末尾两个
                    if (args.current != args.pageCount && args.current < args.pageCount - 2 && args.pageCount > 9) {
                        obj.append('<a href="javascript:;" class="tcdNumber">' + (args.pageCount - 1) + '</a>');
                        obj.append('<a href="javascript:;" class="tcdNumber">' + args.pageCount + '</a>');
                    }
                } else {//简单模式两种
                    obj.append('<span class="simcur">' + args.current + '</span>');
                }

                //下一页
                if (args.current < args.pageCount) {
                    obj.append('<a href="javascript:;" class="nextPage">></a>');
                } else {
                    obj.remove('.nextPage');
                    obj.append('<a href="javascript:;" class="disabled disRight">></a>');
                }

                //首页
                if (args.type === "normal") {
                    if (args.current < args.pageCount) {
                        obj.append('<a href="javascript:;" class="endPage">>></a>');
                    } else {
                        obj.remove('.endPage');
                        obj.append('<a href="javascript:;" class="disabled endPage">></a>');
                    }
                }

                obj.append('<input type="text" name="name" class="changeNum"/><span class="pageCount">/' + args.pageCount + '</span> <a href="javascript:;" class="changeBtn">跳转</a>');
            })();
        },
        //绑定事件
        bindEvent: function (obj, args) {
            return (function () {
                obj.on("click", "a.tcdNumber", function () {
                    var current = parseInt($(this).text());
                    pg.fillHtml(obj, {
                        "current": current, "pageCount": args.pageCount, "type": args.type, "pageSizeShow": args.pageSizeShow, "pageSize": args.pageSize});
                    if (typeof (args.backFn) == "function") {
                        args.backFn(current);
                    }
                });
                //上一页
                obj.on("click", "a.prevPage", function () {
                    var index = parseInt(obj.children("a.current").text());
                    var current = index > 0 ? index : parseInt(obj.children("span.simcur").text());
                    pg.fillHtml(obj, { "current": current - 1, "pageCount": args.pageCount, "type": args.type, "pageSizeShow": args.pageSizeShow, "pageSize": args.pageSize });
                    if (typeof (args.backFn) == "function") {
                        args.backFn(current - 1);
                    }
                });
                //下一页
                obj.on("click", "a.nextPage", function () {
                    var index = parseInt(obj.children("a.current").text())
                    var current = index > 0 ? index : parseInt(obj.children("span.simcur").text());
                    pg.fillHtml(obj, { "current": current + 1, "pageCount": args.pageCount, "type": args.type, "pageSizeShow": args.pageSizeShow, "pageSize": args.pageSize });
                    if (typeof (args.backFn) == "function") {
                        args.backFn(current + 1);
                    }
                });

                //首页
                obj.on("click", "a.firstPage", function () {
                    pg.fillHtml(obj, { "current": 1, "pageCount": args.pageCount, "type": args.type, "pageSizeShow": args.pageSizeShow, "pageSize": args.pageSize });
                    if (typeof (args.backFn) == "function") {
                        args.backFn(1);
                    }
                });

                //尾页
                obj.on("click", "a.endPage", function () {
                    pg.fillHtml(obj, { "current": args.pageCount, "pageCount": args.pageCount, "type": args.type, "pageSizeShow": args.pageSizeShow, "pageSize": args.pageSize });
                    if (typeof (args.backFn) == "function") {
                        args.backFn(args.pageCount);
                    }
                });

                obj.on("click", "a.changeBtn", function () {
                    var current = parseInt(obj.children("input.changeNum").val());
                    if (!current || current < 0 || current > args.pageCount) {
                        obj.children("input.changeNum").val('');
                        return;
                    }
                    pg.fillHtml(obj, { "current": current, "pageCount": args.pageCount, "type": args.type });
                    if (typeof (args.backFn) == "function") {
                        args.backFn(current);
                    }
                })
            })();
        }
    }
    $.fn.createPage = function (options) {
        var args = $.extend({
            totalCount: 20,
            current: 1,
            pageSize: 20,
            pageCount: 0,
            type: 'complex',
            pageSizeShow: 'hide',
            customClass: '',
            backFn: function () { }
        }, options);
        //计算页数
        args.pageCount = Math.ceil(args.totalCount / args.pageSize);

        $(this).html('');

        this.append('<div class="tcdPageCode ' + args.customClass + '"></div>');
        var curcont = this.find('.tcdPageCode');
        pg.init(curcont, args);
    }
})(jQuery);