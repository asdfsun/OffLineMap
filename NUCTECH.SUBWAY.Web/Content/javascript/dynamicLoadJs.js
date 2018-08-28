; (function () {
    var Bmap = function () {
        var thiz = this;
    }

    Bmap.prototype = {
        init: function () {
            var thiz = this;
            thiz.loadJScript();
        },
        loadJScript: function () {
            var thiz = this;
            //var online = navigator.onLine;//navigator.onLine 布尔值 判断离线在线 [不准]
            var online = $('#isOnLine').val();;
            if (online == 1) {
                var script = document.createElement("script");
                script.type = "text/javascript";
                script.src = "http://api.map.baidu.com/api?v=2.0&ak=Eo4HOhcZR8sWP9ndPkvs1psUDhmILKot&callback=empty";
                //script.src = "http://api.map.baidu.com/api?v=2.0&ak=Eo4HOhcZR8sWP9ndPkvs1psUDhmILKot";
                document.body.appendChild(script);
                setTimeout(thiz.proJs, 500);
            } else {
                var script = document.createElement("script");
                script.type = "text/javascript";
                script.src = thiz.getRootPath() + "/Content/javascript/offlineBMap/js/apiv2.0.min.js";
                document.body.appendChild(script);
                setTimeout(thiz.getmodules, 300);
                setTimeout(thiz.proJs, 500);
            }
        },
        getmodules: function () {
            var path = window.document.location.href;
            var pathname = window.document.location.pathname;
            var position = path.indexOf(pathname);
            var rootpath = path.substring(0, position);

            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = rootpath + "/Content/javascript/offlineBMap/js/getmodules.js";
            document.body.appendChild(script);
        },
        empty: function () {
        },
        proJs: function () {
            var path = window.document.location.href;
            var pathname = window.document.location.pathname;
            var position = path.indexOf(pathname);
            var rootpath = path.substring(0, position);

            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = rootpath + "/Content/javascript/mapMonitorList.js";
            document.body.appendChild(script);
        },
        getRootPath: function () {
            var path = window.document.location.href;
            var pathname = window.document.location.pathname;
            var position = path.indexOf(pathname);
            var rootpath = path.substring(0, position);
            return (rootpath);
        }
    }

    $(document).ready(function () {
        var mp = new Bmap();
        window.onload = mp.init();;
    });
})(jQuery)