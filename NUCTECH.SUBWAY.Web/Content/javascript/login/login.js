; (function ($) {
    function jsInfo() {
        var thiz = this;
    }
    jsInfo.prototype = {
        init: function () {
            var thiz = this;

            if (window.localStorage) {
                var userName = localStorage.getItem("account");
                $("#Account").val(userName);
            }

            $("#btnLogin").click(function () {
                thiz.login();
            });
             
            document.onkeydown = function (event) {
                var e = event || window.event || arguments.callee.caller.arguments[0];
                if (e && e.keyCode === 13) { // enter 键
                    thiz.login();
                }
            };
            thiz.remandUrl = thiz.getQueryStringByName('returnUrl');


        },
        login: function () {
            var thiz = this;
            var userName = $("#Account").val();
            var password = $("#Password").val();
            thiz.clearTitle();
            if (userName.length === 0) {
                thiz.setAccountTitle("请输入账号！")
                return;
            }
            if (password.length === 0) {
                thiz.setPasswordTitle("请输入密码！")
                return;
            }
            if ($("#btnLogin").val() === "登  录") {

                if (window.localStorage) {
                    if ($('#isRemember').is(':checked')) {
                        localStorage.setItem("account", userName);
                    } else {
                        localStorage.removeItem("account");
                    }
                }
                $.ajax({
                    url: "../../../Account/Login",
                    type: "POST",
                    data: { UserName: userName, Password: password, isRemember: "on", returnUrl: thiz.remandUrl },
                    beforeSend: function () {
                        $("#btnLogin").val("登录中...");
                    },
                    success: function (res) {
                        if (res.Status) {
                            $("#btnLogin").val("跳转中...");
                            window.location.href = res.ReturnUrl;
                        } else {
                            var s = res.ReMsg.split('|');
                            if (s[0] === "1") {
                                thiz.setAccountTitle(s[1]);
                            } else {
                                thiz.setPasswordTitle(s[1]);
                            }
                            $("#btnLogin").val("登  录");
                            $("#Password").val("");
                        }

                    }, error: function (e) {
                    }
                });
            }
        },
        setAccountTitle: function (msg) {
            $("#accountLi").addClass("warning");
            $("#accountTitle").text(msg);
            setTimeout(function () {
                $("#accountTitle").text("");
            }, 3000)
        },
        setPasswordTitle: function (msg) {
            $("#passwordLi").addClass("warning");
            $("#passwordTitle").text(msg);
            setTimeout(function () {
                $("#passwordTitle").text("");
            }, 3000)
        },
        clearTitle: function () {
            $("#accountLi").removeClass("warning");
            $("#accountTitle").text("");
            $("#passwordLi").removeClass("warning");
            $("#passwordTitle").text("");
        },
        getQueryStringByName: function (name) {
            var _this = this;
            var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
            if (result == null || result.length < 1) {
                return "";

            }
            return result[1];
        },
    }
    $(document).ready(function () {
        var js = new jsInfo();
        js.init();
    });
})(jQuery);