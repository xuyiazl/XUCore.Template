
var editormdInit = function (selector) {
    editormd.emoji = {
        path: "https://www.webfx.com/tools/emoji-cheat-sheet/graphics/emojis/",
        ext: ".png"
    };
    editormd.twemoji = {
        path: "http://twemoji.maxcdn.com/72x72/",
        ext: ".png"
    };
    var editor = editormd(selector, {
        width: "100%",
        height: "640px",
        autoHeight: false,
        autoFocus: false,
        path: "/lib/editor.md/lib/",
        pluginPath: "/lib/editor.md/plugins/",
        htmlDecode: "style,script,iframe",
        saveHTMLToTextarea: true,
        tex: true,
        emoji: true,
        taskList: true,
        flowChart: true,
        sequenceDiagram: true,
        tocm: true,
        tocContainer: "",
        tocDropdown: true,
        tocTitle: "目录索引",
        imageUpload: true,
        imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
        imageUploadURL: "/api/upload/image",
        toolbarIcons: function () {
            return editormd.toolbarModes.full.concat([]);
        }
    });

    return editor;
}

var editormdSimpleInit = function (selector) {
    editormd.emoji = {
        path: "https://www.webfx.com/tools/emoji-cheat-sheet/graphics/emojis/",
        ext: ".png"
    };
    editormd.twemoji = {
        path: "http://twemoji.maxcdn.com/72x72/",
        ext: ".png"
    };
    var editor = editormd(selector, {
        width: "100%",
        height: "400px",
        path: "/lib/editor.md/lib/",
        pluginPath: "/lib/editor.md/plugins/",
        htmlDecode: "style,script,iframe",
        saveHTMLToTextarea: true,
        autoFocus :false,
        toolbarIcons: function () {
            return ["bold", "del", "italic", "code-block"];
        },
        onload: function () {
            this.unwatch();
        }
    });

    return editor;
}