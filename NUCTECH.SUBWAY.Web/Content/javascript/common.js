/*
 * js公共方法
 * version: 1.0.0
 */
//字符串清空所有空格
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
};
//字符串清空左边空格
String.prototype.ltrim = function () {
    return this.replace(/(^\s*)/g, "");
};
//字符串清空右边空格
String.prototype.rtrim = function () {
    return this.replace(/(\s*$)/g, "");
};

(function (window, undefined) {

    CTK = window.CTK || {};
    //清除所有输入框空格的
    CTK.trimBorderSpace = function () {
        $("body").delegate("input[type='text']", 'blur', function () {
            $(this).val($(this).val().ltrim().rtrim().replace(/\s+/g, ' '));
        });
    };
    /*
    超大数字处理
    maxNum:最大数字值
    points:保留小数位
    userNum:用户输入的数值
    unit:处理后的单位
    */
    CTK.largeNumFormat = function (maxNum, points, userNum, unit) {
        if (userNum > maxNum) {
            return (userNum / maxNum).toFixed(points) + unit;
        }
        return userNum;
    };
    /**
     * 绑定表单验证方法
     * @method bindValidation
     * @for 
     * @param { string } formId 表单ID
     * @example bindValidation("form1")
     * @return
     */
    CTK.specificCharacterLength = function (field, rules, i, options) {
        if (field.val().length != 15) {
            return $.i18n.map.specificCharacterLength;
        }
    };
    CTK.bindValidation = function (formId, tipPos, isScroll) {
        var tip = tipPos ? tipPos : 'bottomLeft';
        isScroll = isScroll === 0 ? false : true;
        $(formId).validationEngine({
            scroll: isScroll,//屏幕自动滚动到第一个验证不通过的位置。
            focusFirstField: true,
            promptPosition: tip,
            maxErrorsPerField: 1,
            autoHidePrompt: true,
            autoHideDelay: 30000,
            showArror: false,
            addFailureCssClassToField: "validatefieldClass"
        });
        CTK.trimBorderSpace();
    };
    //表单中第一个非隐藏域获得焦点
    CTK.getTheFirstFocus = function () {
        var form = $("form");
        if (form.length > 0) {
            var oInput = $("input", form);
            for (var i = 0; i < oInput.length; i++) {
                var oField = $(oInput[i]);
                if (oField.attr("type") !== 'hidden') {
                    oField.focus();
                    break;
                }
            }
        }
    };
    //阻止事件冒泡
    CTK.stopBubble = function (e) {
        var oEvent = e || event;
        if (oEvent.stopPropagation) {
            oEvent.stopPropagation();
        } else if (oEvent.preventDefault) {
            oEvent.preventDefault();
        } else {
            oEvent.cancelBubble = true;
            window.event.returnValue = false;
        }
        return false;
    };

    $(function () {
        CTK.trimBorderSpace();
    });
    window.CTK = CTK;
})(window);



