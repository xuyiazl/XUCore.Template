
var tinymceInit = function (selector) {

    //http://tinymce.ax-z.cn/

    tinymce.init({
        selector: selector,
        language: 'zh_CN',
        //icons: 'custom',
        branding: false,
        plugins: 'print quickbars preview searchreplace autolink directionality visualblocks visualchars fullscreen image imagetools link media template code codesample table charmap hr pagebreak nonbreaking anchor insertdatetime advlist lists wordcount textpattern emoticons help paste autosave',//
        toolbar: 'fontselect fontsizeselect | alignleft aligncenter alignright alignjustify outdent indent | bullist numlist |\
                  image link media | codesample code preview fullscreen',
        quickbars_selection_toolbar: 'forecolor backcolor | bold italic underline strikethrough subscript superscript removeformat | quicklink h2 h3 h4 blockquote',
        imagetools_toolbar: "rotateleft rotateright | flipv fliph | editimage imageoptions",
        height: 640,
        fontsize_formats: '12px 14px 16px 18px 24px 36px 48px 56px 72px',
        font_formats: '微软雅黑=Microsoft YaHei,Helvetica Neue,PingFang SC,sans-serif;苹果苹方=PingFang SC,Microsoft YaHei,sans-serif;宋体=simsun,serif;仿宋体=FangSong,serif;黑体=SimHei,sans-serif;Arial=arial,helvetica,sans-serif;Arial Black=arial black,avant garde;Book Antiqua=book antiqua,palatino;Comic Sans MS=comic sans ms,sans-serif;Courier New=courier new,courier;Georgia=georgia,palatino;Helvetica=helvetica;Impact=impact,chicago;Symbol=symbol;Tahoma=tahoma,arial,helvetica,sans-serif;Terminal=terminal,monaco;Times New Roman=times new roman,times;Verdana=verdana,geneva;Webdings=webdings;Wingdings=wingdings,zapf dingbats;知乎配置=BlinkMacSystemFont, Helvetica Neue, PingFang SC, Microsoft YaHei, Source Han Sans SC, Noto Sans CJK SC, WenQuanYi Micro Hei, sans-serif;小米配置=Helvetica Neue,Helvetica,Arial,Microsoft Yahei,Hiragino Sans GB,Heiti SC,WenQuanYi Micro Hei,sans-serif',
        codesample_languages: [
            { text: 'HTML/XML/Markup/SVG/MathML', value: 'markup' },
            { text: 'BASH/SHELL', value: 'bash' },
            { text: 'CSS', value: 'css' },
            { text: 'SASS', value: 'sass' },
            { text: 'SCSS', value: 'scss' },
            { text: 'CLike', value: 'clike' },
            { text: 'JavaScript', value: 'javascript' },
            { text: 'TypeScript', value: 'typescript' },
            { text: 'JSON', value: 'json' },
            { text: 'ts', value: 'ts' },
            { text: 'C', value: 'c' },
            { text: 'C#', value: 'csharp' },
            { text: 'ASPNET', value: 'aspnet' },
            { text: 'DOTNET', value: 'dotnet' },
            { text: 'CoffeeScript', value: 'coffeescript' },
            { text: 'Coffee', value: 'coffee' },
            { text: 'Go', value: 'go' },
            { text: 'Less', value: 'less' },
            { text: 'Java', value: 'java' },
            { text: 'PHP', value: 'php' },
            { text: 'markdown', value: 'markdown' },
            { text: 'md', value: 'md' },
            { text: 'Objective-C', value: 'objectivec' },
            { text: 'Perl', value: 'perl' },
            { text: 'SQL', value: 'sql' },
            { text: 'Python', value: 'python' },
            { text: 'py', value: 'py' },
            { text: 'jsx', value: 'jsx' },
            { text: 'tsx', value: 'tsx' },
            { text: 't4-templating', value: 't4-templating' },
            { text: 't4-cs', value: 't4-cs' },
            { text: 't4', value: 't4' },
            { text: 'Regex', value: 'regex' }
        ],
        codesample_content_css: '/tinymce/prism.css',
        //images_upload_url: '/api/upload/image?editor=tinymce',
        images_upload_handler: function (blobInfo, success, failure) {
            var xhr, formData;
            xhr = new XMLHttpRequest();
            xhr.withCredentials = false;
            xhr.open('POST', '/api/upload/image?editor=tinymce');
            xhr.onload = function () {
                var json;
                if (xhr.status != 200) {
                    failure('HTTP Error: ' + xhr.status);
                    return;
                }
                json = JSON.parse(xhr.responseText);
                if (!json || typeof json.Data.RootUrl != 'string') {
                    failure('Invalid JSON: ' + xhr.responseText);
                    return;
                }
                success(json.Data.RootUrl);
            };
            formData = new FormData();
            formData.append('file', blobInfo.blob(), blobInfo.filename());
            xhr.send(formData);
        },
        file_picker_types: 'image',
        file_picker_callback: function (callback, value, meta) {
            if (meta.filetype === 'image') {

                var input = document.createElement('input');
                input.setAttribute('type', 'file');
                input.setAttribute('accept', 'image/*');

                input.onchange = function () {
                    var file = this.files[0];

                    var reader = new FileReader();
                    reader.onload = function () {
                        $.ajax({
                            url: '/api/upload/base64image',
                            type: "POST",
                            data: { Base64: reader.result, type: file.type },
                            success: function (res) {
                                callback(res.Data.RootUrl, { title: file.name });
                            }
                        });
                    };
                    reader.readAsDataURL(file);
                };

                input.click();
            }
        }
    });
}



var tinymceSimpleInit = function (selector) {

    //http://tinymce.ax-z.cn/

    tinymce.init({
        selector: selector,
        language: 'zh_CN',
        icons: 'custom',
        toolbar: false,
        menubar:false,
        branding: false,
        plugins: 'codesample',//
        toolbar: 'fontsizeselect | forecolor backcolor bold italic underline strikethrough | alignleft aligncenter alignright | blockquote codesample',
        quickbars_selection_toolbar: 'forecolor backcolor | bold italic underline strikethrough subscript superscript removeformat | quicklink h2 h3 h4 blockquote',
        height: 340,
        fontsize_formats: '14px 16px 18px',
        font_formats: '微软雅黑=Microsoft YaHei,Helvetica Neue,PingFang SC,sans-serif;苹果苹方=PingFang SC,Microsoft YaHei,sans-serif;宋体=simsun,serif;仿宋体=FangSong,serif;黑体=SimHei,sans-serif;Arial=arial,helvetica,sans-serif;Arial Black=arial black,avant garde;Book Antiqua=book antiqua,palatino;Comic Sans MS=comic sans ms,sans-serif;Courier New=courier new,courier;Georgia=georgia,palatino;Helvetica=helvetica;Impact=impact,chicago;Symbol=symbol;Tahoma=tahoma,arial,helvetica,sans-serif;Terminal=terminal,monaco;Times New Roman=times new roman,times;Verdana=verdana,geneva;Webdings=webdings;Wingdings=wingdings,zapf dingbats;知乎配置=BlinkMacSystemFont, Helvetica Neue, PingFang SC, Microsoft YaHei, Source Han Sans SC, Noto Sans CJK SC, WenQuanYi Micro Hei, sans-serif;小米配置=Helvetica Neue,Helvetica,Arial,Microsoft Yahei,Hiragino Sans GB,Heiti SC,WenQuanYi Micro Hei,sans-serif',
        codesample_languages: [
            { text: 'HTML/XML/Markup/SVG/MathML', value: 'markup' },
            { text: 'BASH/SHELL', value: 'bash' },
            { text: 'CSS', value: 'css' },
            { text: 'SASS', value: 'sass' },
            { text: 'SCSS', value: 'scss' },
            { text: 'CLike', value: 'clike' },
            { text: 'JavaScript', value: 'javascript' },
            { text: 'TypeScript', value: 'typescript' },
            { text: 'JSON', value: 'json' },
            { text: 'ts', value: 'ts' },
            { text: 'C', value: 'c' },
            { text: 'C#', value: 'csharp' },
            { text: 'ASPNET', value: 'aspnet' },
            { text: 'DOTNET', value: 'dotnet' },
            { text: 'CoffeeScript', value: 'coffeescript' },
            { text: 'Coffee', value: 'coffee' },
            { text: 'Go', value: 'go' },
            { text: 'Less', value: 'less' },
            { text: 'Java', value: 'java' },
            { text: 'PHP', value: 'php' },
            { text: 'markdown', value: 'markdown' },
            { text: 'md', value: 'md' },
            { text: 'Objective-C', value: 'objectivec' },
            { text: 'Perl', value: 'perl' },
            { text: 'SQL', value: 'sql' },
            { text: 'Python', value: 'python' },
            { text: 'py', value: 'py' },
            { text: 'jsx', value: 'jsx' },
            { text: 'tsx', value: 'tsx' },
            { text: 't4-templating', value: 't4-templating' },
            { text: 't4-cs', value: 't4-cs' },
            { text: 't4', value: 't4' },
            { text: 'Regex', value: 'regex' }
        ],
        codesample_content_css: '/tinymce/prism.css'
    });
}

