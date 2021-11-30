﻿
var editormdPreviewInit = function (selector) {
    editormd.emoji = {
        path: "https://www.webfx.com/tools/emoji-cheat-sheet/graphics/emojis/",
        ext: ".png"
    };
    editormd.twemoji = {
        path: "http://twemoji.maxcdn.com/72x72/",
        ext: ".png"
    };
    var editor = editormd.markdownToHTML(selector, {
        //htmlDecode      : true,       // 开启 HTML 标签解析，为了安全性，默认不开启
        htmlDecode: "style,script,iframe",  // you can filter tags decode
        toc: true,
        tocm: true,    // Using [TOCM]
        //tocContainer    : "#custom-toc-container", // 自定义 ToC 容器层
        //gfm             : false,
        //tocDropdown     : true,
        // markdownSourceCode : true, // 是否保留 Markdown 源码，即是否删除保存源码的 Textarea 标签
        emoji: true,
        taskList: true,
        tex: true,  // 默认不解析
        flowChart: true,  // 默认不解析
        sequenceDiagram: true,  // 默认不解析
    });
    return editor;
}