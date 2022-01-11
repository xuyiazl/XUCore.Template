/*!
 * gears dialog plugin for Editor.md
 *
 * @file        gears-dialog.js
 * @author      pandao
 * @version     1.2.0
 * @updateTime  2015-03-08
 * {@link       https://github.com/pandao/editor.md}
 * @license     MIT
 */

(function () {

    var factory = function (exports) {

        var $ = jQuery;
        var pluginName = "gears-dialog";

        exports.fn.gearsDialog = function () {
            var _this = this;
            var lang = this.lang;
            var cm = this.cm;
            var editor = this.editor;
            var settings = this.settings;
            var cursor = cm.getCursor();
            var selection = cm.getSelection();
            var path = settings.pluginPath + pluginName + "/";
            var classPrefix = this.classPrefix;
            var dialogName = classPrefix + pluginName, dialog;
            var dialogLang = lang.dialog.gears;

            if (editor.find("." + dialogName).length < 1) {
                var dialogContent = "<div class=\"markdown-body\" style=\"font-family:微软雅黑, Helvetica, Tahoma, STXihei,Arial;height:170px;overflow:auto;font-size:14px;border-bottom:1px solid #ddd;padding:0 20px 20px 0;\"></div>";

                dialog = this.createDialog({
                    name: dialogName,
                    title: dialogLang.title,
                    width: 400,
                    height: 300,
                    mask: settings.dialogShowMask,
                    drag: settings.dialogDraggable,
                    content: dialogContent,
                    lockScreen: settings.dialogLockScreen,
                    maskStyle: {
                        opacity: settings.dialogMaskOpacity,
                        backgroundColor: settings.dialogMaskBgColor
                    },
                    buttons: {
                        close: [lang.buttons.close, function () {
                            this.hide().lockScreen(false).hideMask();

                            return false;
                        }]
                    }
                });
            }

            dialog = editor.find("." + dialogName);

            this.dialogShowMask(dialog);
            this.dialogLockScreen();

            dialog.show();

            var gearsContent = dialog.find(".markdown-body");

            if (gearsContent.html() === "") {

                gearsContent.html('\
                    <a href="javascript:;" data-src="'+ path + 'tmpl/api.md">[Api模板]</a>\
                    <a href="javascript:;" data-src="'+ path + 'tmpl/dict.md">[数据字典模板]</a>\
                    <a href="javascript:;" data-src="'+ path + 'tmpl/demo.md">[Demo模板]</a>\
                ');

                gearsContent.find('a').bind(exports.mouseOrTouch("click", "touchend"), function () {
                    var __this = $(this);
                    $.get(__this.data('src'), function (text) {
                        cm.replaceSelection(text);
                        dialog.hide();
                        editor.find("." + classPrefix + "mask").hide();
                    });
                });
            }
        };

    };

    // CommonJS/Node.js
    if (typeof require === "function" && typeof exports === "object" && typeof module === "object") {
        module.exports = factory;
    }
    else if (typeof define === "function")  // AMD/CMD/Sea.js
    {
        if (define.amd) { // for Require.js

            define(["editormd"], function (editormd) {
                factory(editormd);
            });

        } else { // for Sea.js
            define(function (require) {
                var editormd = require("./../../editormd");
                factory(editormd);
            });
        }
    }
    else {
        factory(window.editormd);
    }

})();

