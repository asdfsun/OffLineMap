; (function ($) {
    function jsInfo() {
        var thiz = this;
    }
    jsInfo.prototype = {
        init: function () {
            var thiz = this;

            thiz.getMenuInfo();
            $("#cancel").click(function () {
                var dg = $.dialog({
                    width: 420, //弹出框的宽度,具体到像素,不用加单位,(默认为300px,可不传)
                    title: '退出提示',
                    content: "您确认要退出登录吗？",
                    onOk: function () {
                        window.location.href = "../../../Account/LoginOut";
                    }
                });
            });
            $("#resetPwd").click(function () {
                $.get("../../../SystemManage/User/ResetPwd", {}, function (html) {
                    var contHtml = $(html);
                    var dg = $.dialog({
                        width: 420, //弹出框的宽度,具体到像素,不用加单位,(默认为300px,可不传)
                        title: '修改密码',
                        content: contHtml,
                        onOk: function () {
                            //  CTK.bindValidation($("#resetPwdForm"));
                            //  if ($("#resetPwdForm").validationEngine('validate')) {

                            if ($("#oldPwd").val().length == 0) {
                                $("#pass1").text("*请输入旧密码！");
                                $("#pass1").show();
                                setTimeout(function () {
                                    $("#pass1").hide();
                                    $("#pass2").hide();
                                    $("#pass3").hide();
                                }, 4000);
                                return;
                            }
                            if ($("#oldPwd").val().indexOf(' ') != -1) {
                                $("#pass1").text("*密码不可以包含空格！");
                                $("#pass1").show();
                                setTimeout(function () {
                                    $("#pass1").hide();
                                    $("#pass2").hide();
                                    $("#pass3").hide();
                                }, 4000);
                                return;
                            }


                            if ($("#newPwd").val().length == 0) {
                                $("#pass2").text("*请输入新密码！");
                                $("#pass2").show();
                                setTimeout(function () {
                                    $("#pass1").hide();
                                    $("#pass2").hide();
                                    $("#pass3").hide();
                                }, 4000);
                                return;
                            }

                            if ($("#newPwd").val().indexOf(' ') != -1) {
                                $("#pass2").text("*新密码不可以包含空格！");
                                $("#pass2").show();
                                setTimeout(function () {
                                    $("#pass1").hide();
                                    $("#pass2").hide();
                                    $("#pass3").hide();
                                }, 3000);
                                return;
                            }

                            if ($("#newPwd2").val().length == 0 && thiz.checkTextSpace($("#newPwd2"))) {
                                $("#pass3").show();
                                setTimeout(function () {
                                    $("#pass1").hide();
                                    $("#pass2").hide();
                                    $("#pass3").hide();
                                }, 3000);
                                return;
                            }
                            if ($("#newPwd").val() != $("#newPwd2").val()) {
                                $("#pass3").show();
                                setTimeout(function () {
                                    //$("#newPwd").val("");
                                    //$("#newPwd2").val("");
                                    $("#resetPwdTitle").hide();
                                }, 3000);
                                return;
                            }
                            $.ajax({
                                url: "../../../SystemManage/User/ResetPwd",
                                type: "post",
                                data: { newPassword: $("#newPwd").val(), oldPassword: $("#oldPwd").val() },
                                success: function (data) {
                                    if (data.Status) {
                                        dg.close();
                                        $.dialog({
                                            dialogType: 'info',
                                            width: 320, //弹出框的宽度,具体到像素,不用加单位,(默认为300px,可不传)
                                            title: '密码修改成功,请重新登录系统!',
                                            infoBg: 'green',
                                            content: '密码修改成功,请重新登录系统!',
                                            onOk: function () {
                                                window.location.href = "../../../Account/Login";
                                            }
                                        });

                                    }
                                    else {
                                        dg.close();
                                        $.dialog({
                                            dialogType: 'info',
                                            isAutoClose: false,
                                            width: 420, //弹出框的宽度,具体到像素,不用加单位,(默认为300px,可不传)
                                            title: '密码修改失败，' + data.ReMsg,
                                            content: '',
                                            infoBg: 'orange',
                                        });
                                    }
                                }
                            });
                            //   }
                        }
                    })
                });
            });
            try {
                if (browserEvent != null && browserEvent != undefined) {
                    $("#shutdown").click(function () {
                        var dg = $.dialog({
                            width: 420, //弹出框的宽度,具体到像素,不用加单位,(默认为300px,可不传)
                            title: '关机提示',
                            content: "您确认要关闭计算机吗？",
                            onOk: function () {
                                browserEvent.shutdown();
                            }
                        });
                    });
                } else {

                }
            } catch (e) {
                $("#shutdown").hide();
            }

            setInterval(function () {
                var date = new Date();
                var month = date.getMonth() + 1;
                if (month >= 1 && month <= 9) {
                    month = "0" + month;
                }
                var day = date.getDate();
                if (day >= 1 && day <= 9) {
                    day = "0" + day;
                }
                var hour = date.getHours();
                if (hour >= 1 && hour <= 9) {
                    hour = "0" + hour;
                }
                var minute = date.getMinutes();
                if (minute >= 1 && minute <= 9) {
                    minute = "0" + minute;
                }

                $("#time").text(hour + ":" + minute);
                $("#date").text(month + "/" + day);
            }, 1000);
            try {
                var url = window.location.href;
                if (url.lastIndexOf('=') > 0) {
                    var u = url.substr(url.lastIndexOf('=') + 1);
                    var v = $(".navBar .navBtn");
                    for (var i = 0; i < v.length; i++) {
                        var treeId = v[i].attributes["u"].value;
                        if (treeId == u)
                            $(v[i]).addClass('on');
                    }
                }
            } catch (e) {
            }
        },
        checkPwd: function (s) {
            var thiz = this;
            var regu = /[<>\/'[\]]/;
            var re = new RegExp(regu);
            if (re.test(s)) {
                return true;
            } else {
                return false;
            }
        },
        checkTextSpace: function (obj) {
            var thiz = this;
            var reg = /(^\s+)|(\s+$)/g;
            var alertValue = "输入内容包含空格，请重新输入!";
            if (reg.test(obj.value)) {
                alert(alertValue);
                obj.focus();
                return false;
            }
        },
        getMenuInfo: function () {
            var thiz = this;

            $.ajax({
                url: '../Home/getMenuInfo',
                type: 'get',
                success: function (data) {
                    if (data != '') {
                        var menuHtmParent = '';
                       
                        if (data.TreeInfo.length > 0) {


                            for (var i = 0; i < data.TreeInfo.length; i++) {
                                if (data.TreeInfo[i].ParentID == 0) {
                                    menuHtmParent += '<li class="navBtn" u="' + data.TreeInfo[i].TreeId + '"><span>' + data.TreeInfo[i].TreeName + '</span><div class="underline"></div><div class="secondLevel">'

                                    var menuHtmChild = '';
                                    menuHtmChild += '<ul>';
                                    for (var j = 0; j < data.TreeInfo.length; j++) {
                                        if (data.TreeInfo[j].ParentID == data.TreeInfo[i].TreeId)
                                        {
                                            menuHtmChild += '<li><a href="' + data.TreeInfo[j].TreeUrl + '">' + data.TreeInfo[j].TreeName + '</a></li>'
                                        }
                                      
                                    }
                                    menuHtmChild += '</div></ul>';

                                    menuHtmParent += menuHtmChild;

                                    menuHtmParent += '</li>';
                                }

                            }
                        }

                        $('#navBar').empty().append(menuHtmParent);
                    }
                }
            });

        }
    }
    $(document).ready(function () {
        var js = new jsInfo();
        js.init();
    });
})(jQuery);

