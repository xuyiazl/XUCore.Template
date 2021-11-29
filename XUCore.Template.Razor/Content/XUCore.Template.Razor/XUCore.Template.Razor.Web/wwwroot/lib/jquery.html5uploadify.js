; (function ($) {
    $.fn.uploadify = function (opts) {
        var defaults = {
            fileTypeExts: '*.*',//允许上传的文件类型，格式'*.jpg;*.doc'
            capture: 'camera',//capture表示，可以捕获到系统默认的设备，比如：camera--照相机；camcorder--摄像机；microphone--录音。
            uploader: '',//文件提交的地址
            auto: false,//是否开启自动上传
            method: 'POST',//发送请求的方式，get或post
            multi: true,//是否允许选择多个文件,开启后 对手机的照相功能失效
            before: false,//在按钮之前插入模板
            formData: null,//发送给服务端的参数，格式：{key1:value1,key2:value2}
            fileObjName: 'imgFile',//在后端接受文件的参数名称，如PHP中的$_FILES['file']
            fileSizeLimit: 2048,//允许上传的文件大小，单位KB
            itemTemplate: '',//上传队列显示的模板
            queue: '#upload_queue',//队列容器
            preview: false,//是否启用预览
            previewMode: 'cut',//预览裁剪方式  cut or wh
            previewMaxWidth: 800,//预览的最大宽度
            previewMaxHeight: 600,//预览的最大高度
            onPreview: null,//预览返回动作--等canvas加载完毕后
            onUploadStart: null,//上传开始时的动作
            onUploadSuccess: null,//上传成功的动作
            onUploadComplete: null,//上传完成的动作
            onProgress: null,//上传进度
            onUploadError: null, //上传失败的动作
            onInit: null,//初始化时的动作
            onCancel: null,//删除掉某个文件后的回调函数，可传入参数file
            onClearQueue: null,//清空上传队列后的回调函数，在调用cancel并传入参数*时触发
            onDestroy: null,//在调用destroy方法时触发
            onSelect: null,//选择文件后的回调函数，可传入参数file
            onQueueComplete: null//队列中的所有文件上传完成后触发
        }

        var option = $.extend(defaults, opts);

        //定义一个通用函数集合
        var F = {
            //将文件的单位由bytes转换为KB或MB，若第二个参数指定为true，则永远转换为KB
            formatFileSize: function (size, withKB) {
                if (size > 1024 * 1024 && !withKB) {
                    size = (Math.round(size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
                }
                else {
                    size = (Math.round(size * 100 / 1024) / 100).toString() + 'KB';
                }
                return size;
            },
            //将输入的文件类型字符串转化为数组,原格式为*.jpg;*.png
            getFileTypes: function (str) {
                var result = [];
                var arr1 = str.split(";");
                for (var i = 0, len = arr1.length; i < len; i++) {
                    result.push(arr1[i].split(".").pop());
                }
                return result;
            },
            ////根据文件序号获取文件
            getFile: function (index, files) {
                for (var i = 0; i < files.length; i++) {
                    if (files[i].index == index) {
                        return files[i];
                    }
                }
                return null;
            }
        };

        var returnObj = null;

        this.each(function (index, element) {
            var _this = $(element);
            var _queue = $(option.queue);
            var _select = null;
            var mime = { 'png': 'image/png', 'jpg': 'image/jpeg', 'jpeg': 'image/jpeg', 'bmp': 'image/bmp' };
            var uploadManager = {
                filteredFiles: [],//过滤后的文件数组
                fileCount: 0,//存放总数量，删除后也不更新，避免删除后重复的索引
                init: function () {
                    var inputStr = '<input style="display:none;" type="file" capture="' + option.capture + '" name="fileselect[]"';
                    inputStr += option.multi ? ' multiple' : '';
                    inputStr += ' accept="';
                    inputStr += F.getFileTypes(option.fileTypeExts).join(",");
                    inputStr += '"/>';

                    _select = $(inputStr);
                    _this.after(_select);

                    //初始化返回的实例
                    returnObj = {
                        upload: function (fileIndex) {
                            if (fileIndex === '*') {
                                for (var i = 0, len = uploadManager.filteredFiles.length; i < len; i++) {
                                    uploadManager._uploadFile(uploadManager.filteredFiles[i]);
                                    if ($('.start:eq(' + i + ')').length > 0)
                                        $('.start').get(i).disabled = true;
                                }
                            }
                            else {
                                var file = F.getFile(fileIndex, uploadManager.filteredFiles);
                                file && uploadManager._uploadFile(file);
                                if ($('.start:eq(' + fileIndex + ')').length > 0)
                                    $('.start').get(fileIndex).disabled = true;
                            }
                        },
                        cancel: function (fileIndex) {
                            if (fileIndex === '*') {
                                var len = uploadManager.filteredFiles.length;
                                for (var i = len - 1; i >= 0; i--) {
                                    uploadManager._deleteFile(uploadManager.filteredFiles[i]);
                                    if ($('.cancel:eq(' + i + ')').length > 0)
                                        $('.cancel').get(i).disabled = true;
                                }
                                option.onClearQueue && option.onClearQueue(len);
                            }
                            else {
                                var file = F.getFile(fileIndex, uploadManager.filteredFiles);
                                file && uploadManager._deleteFile(file);
                                if ($('.cancel:eq(' + fileIndex + ')').length > 0)
                                    $('.cancel').get(fileIndex).disabled = true;
                            }
                        },
                        disable: function () {
                            element.disabled = true;
                        },
                        ennable: function () {
                            element.disabled = false;
                        },
                        destroy: function () {
                            $(option.queue).remove();
                            _this.remove();
                            option.onDestroy && option.onDestroy();
                        },
                        settings: function (name, value) {
                            if (arguments.length == 1) {
                                return option[name];
                            }
                            else {
                                if (name == 'formData') {
                                    option.formData = $.extend(option.formData, value);
                                }
                                else {
                                    option[name] = value;
                                }
                            }
                        },
                        uploadify: function () {
                            var method = arguments[0];
                            if (method in this) {
                                Array.prototype.splice.call(arguments, 0, 1);
                                this[method].apply(this[method], arguments);
                            }
                        }
                    };

                    //文件选择控件选择
                    if (_select.length > 0) {
                        _select.change(function (e) {
                            uploadManager._getFiles(e);
                        });
                    }

                    //点击选择文件按钮时触发file的click事件
                    _this.on('click', function () {
                        _select.trigger('click');
                        return false;
                    });

                    option.onInit && option.onInit(returnObj);
                },
                _filter: function (files) {		//选择文件组的过滤方法
                    var arr = [];
                    var typeArray = F.getFileTypes(option.fileTypeExts);
                    if (typeArray.length > 0) {
                        for (var i = 0, len = files.length; i < len; i++) {
                            var f = files[i];
                            if (parseInt(F.formatFileSize(f.size, true)) <= option.fileSizeLimit) {
                                if ($.inArray('*', typeArray) >= 0 || $.inArray(f.name.split('.').pop(), typeArray) >= 0) {
                                    arr.push(f);
                                }
                                else {
                                    alert('文件 "' + f.name + '" 类型不允许！');
                                }
                            }
                            else {
                                alert('文件 "' + f.name + '" 大小超出限制！');
                                continue;
                            }
                        }
                    }
                    return arr;
                },
                _itemkey: function (index) {
                    return '.item_' + index;
                },
                //根据选择的文件，渲染DOM节点
                _renderFile: function (file) {
                    var template = $(option.itemTemplate).html();
                    var $html = $(template.replace(/\${fileID}/g, this._itemkey(file.index).replace('.', ''))
                        .replace(/\${fileName}/g, file.name)
                        .replace(/\${fileSize}/g, F.formatFileSize(file.size)));

                    var start = $('.start', $html), cancel = $('.cancel', $html);
                    //如果是非自动上传，显示上传按钮
                    if (option.auto) {
                        start.hide();
                        //cancel.hide();
                    }
                    if (!option.before)
                        _queue.append($html);
                    else
                        _this.before($html);
                    //触发select动作
                    option.onSelect && option.onSelect(file);

                    //判断是否是自动上传
                    if (option.auto) {
                        uploadManager._uploadFile(file);
                    }
                    else {
                        //如果配置非自动上传，绑定上传事件
                        start.on('click', function () {
                            if (!this.disabled) {
                                this.disabled = true;
                                uploadManager._uploadFile(file);
                            }
                        });
                    }
                    //为删除文件按钮绑定删除文件事件
                    cancel.on('click', function () {
                        if (!this.disabled) {
                            this.disabled = true;
                            uploadManager._deleteFile(file);
                        }
                    });

                    this._type = file.type;
                    // 如果没有文件类型，则通过后缀名判断（解决微信及360浏览器无法获取图片类型问题）
                    if (!this._type) {
                        this._type = mime[file.name.match(/\.([^\.]+)$/i)[1]];
                    }

                    if (option.preview)
                        window.setTimeout(function () {
                            uploadManager._readImage(file);
                        }, 0);
                },
                _type: null,
                _readImage: function (file) {
                    var _this = this;

                    function tmpCreateImage(uri) {
                        _this._createImage(file, uri);
                    }
                    //this.loading();

                    this._getURI(file, tmpCreateImage);
                },
                _getURI: function (file, callback) {
                    var reader = new FileReader();
                    var _this = this;
                    function tmpLoad() {
                        // 头不带图片格式，需填写格式
                        var re = /^data:base64,/;
                        var ret = this.result + '';
                        if (re.test(ret)) ret = ret.replace(re, 'data:' + mime[_this._type] + ';base64,');

                        callback && callback(ret);
                    }

                    reader.onload = tmpLoad;

                    reader.readAsDataURL(file);

                    return false;
                },
                _createImage: function (file, uri) {
                    var img = new Image();
                    var _this = this;

                    function tmpLoad() {
                        switch (option.previewMode) {
                            case 'cut':
                                _this._drawImageCut(file, this);
                                break;
                            case 'wh':
                                _this._drawImageWH(file, this);
                                break;
                        }
                    }

                    img.onload = tmpLoad;

                    img.src = uri;
                },
                _drawImageCut: function (file, img) {
                    var _item = $(this._itemkey(file.index), _queue);

                    var canvas = document.createElement('canvas');
                    var ctx = canvas.getContext('2d');

                    var toWidth = option.previewMaxWidth || 800;
                    var toHeight = option.previewMaxHeight || 600;

                    canvas.width = toWidth;
                    canvas.height = toHeight;

                    var sourceX = 0;
                    var sourceY = 0;
                    var sourceWidth = img.width;
                    var sourceHeight = img.height;
                    var destWidth = img.width;
                    var destHeight = img.height;

                    if (sourceWidth / sourceHeight > toWidth / toHeight) {
                        destHeight = sourceHeight;
                        destWidth = sourceHeight * (toWidth / toHeight);
                        destY = 0;
                        if (sourceWidth - toWidth <= 0)
                            destX = 0;
                        else
                            destX = (sourceWidth - destWidth) / 2;
                    }
                    else {
                        destWidth = sourceWidth;
                        destHeight = sourceWidth * (toHeight / toWidth);
                        destX = 0;
                        if (sourceHeight - toHeight <= 0)
                            destY = 0;
                        else
                            destY = (sourceHeight - destHeight) / 2;
                    }
                    ctx.drawImage(img, destX, destY, destWidth, destHeight, 0, 0, toWidth, toHeight);

                    var baseUrl = canvas.toDataURL(this._type);

                    option.onPreview && option.onPreview(file, _item, baseUrl);

                    ctx.clearRect(0, 0, toWidth, toHeight);
                    canvas.width = 0;
                    canvas.height = 0;
                    canvas = null;
                },
                _drawImageWH: function (file, img) {
                    var _item = $(this._itemkey(file.index), _queue);

                    var toWidth = img.width;
                    var toHeight = img.height;
                    var sourceWidth = img.width;
                    var sourceHeight = img.height;

                    var _scale = (sourceWidth / sourceHeight).toFixed(2);

                    if (toWidth > option.previewMaxWidth) {
                        toWidth = option.previewMaxWidth;
                        toHeight = Math.round(toWidth / _scale);
                    }

                    if (toHeight > option.previewMaxHeight) {
                        toHeight = option.previewMaxHeight;
                        toWidth = Math.round(toHeight * _scale);
                    }

                    var canvas = document.createElement('canvas');
                    var ctx = canvas.getContext('2d');

                    canvas.width = toWidth;
                    canvas.height = toHeight;

                    ctx.drawImage(img, 0, 0, sourceWidth, sourceHeight, 0, 0, toWidth, toHeight);

                    var baseUrl = canvas.toDataURL(this._type);

                    option.onPreview && option.onPreview(file, _item, baseUrl);

                    ctx.clearRect(0, 0, toWidth, toHeight);
                    canvas.width = 0;
                    canvas.height = 0;
                    canvas = null;
                },
                //获取选择后的文件
                _getFiles: function (e) {
                    var files = e.target.files;
                    //files = uploadManager._filter(files); //暂时去掉过滤，只用于手机上的上传
                    //var fileCount = $('.queue-item', _queue).length;//队列中已经有的文件个数
                    for (var i = 0, len = files.length; i < len; i++) {
                        files[i].index = ++this.fileCount;
                        files[i].status = 0;//标记为未开始上传
                        uploadManager.filteredFiles.push(files[i]);
                        var l = uploadManager.filteredFiles.length;
                        uploadManager._renderFile(uploadManager.filteredFiles[l - 1]);
                    }
                },
                //删除文件
                _deleteFile: function (file) {
                    for (var i = 0, len = uploadManager.filteredFiles.length; i < len; i++) {
                        var f = uploadManager.filteredFiles[i];
                        if (f.index == file.index) {
                            uploadManager.filteredFiles.splice(i, 1);
                            $(this._itemkey(file.index), _queue).fadeOut(150, function () {
                                $(this).remove();
                            });
                            option.onCancel && option.onCancel(file);
                            break;
                        }
                    }
                },
                //校正上传完成后的进度条误差
                _regulateView: function (file, loaded) {
                    var eleProgress = $(this._itemkey(file.index), _queue);
                    option.onProgress && option.onProgress(eleProgress, F.formatFileSize(file.size), F.formatFileSize(file.size), 100);
                },
                _onProgress: function (file, loaded, total) {
                    var eleProgress = $(this._itemkey(file.index), _queue);
                    var percent = (loaded / total * 100).toFixed(2);
                    option.onProgress && option.onProgress(eleProgress, F.formatFileSize(loaded), F.formatFileSize(total), percent);
                },
                _allFilesUploaded: function () {
                    var queueData = {
                        uploadsSuccessful: 0,
                        uploadsErrored: 0
                    };
                    for (var i = 0, len = uploadManager.filteredFiles.length; i < len; i++) {
                        var s = uploadManager.filteredFiles[i].status;
                        if (s === 0 || s === 1) {
                            queueData = false;
                            break;
                        }
                        else if (s === 2) {
                            queueData.uploadsSuccessful++;
                        }
                        else if (s === 3) {
                            queueData.uploadsErrored++;
                        }
                    }
                    return queueData;
                },
                //上传文件
                _uploadFile: function (file) {
                    var item = $(this._itemkey(file.index), _queue);
                    var xhr = null;
                    try {
                        xhr = new XMLHttpRequest();
                    } catch (e) {
                        xhr = ActiveXobject("Msxml12.XMLHTTP");
                    }
                    if (xhr.upload) {
                        // 上传中
                        xhr.upload.onprogress = function (e) {
                            uploadManager._onProgress(file, e.loaded, e.total);
                        };

                        xhr.onreadystatechange = function (e) {
                            if (xhr.readyState == 4) {
                                if (xhr.status == 200) {
                                    uploadManager._regulateView(file);
                                    file.status = 2;//标记为上传成功
                                    option.onUploadSuccess && option.onUploadSuccess(file, xhr.responseText, item);
                                }
                                else {
                                    file.status = 3;//标记为上传失败
                                    option.onUploadError && option.onUploadError(file, xhr.responseText, item);
                                }
                                option.onUploadComplete && option.onUploadComplete(file, xhr.responseText, item);

                                //检测队列中的文件是否全部上传完成，执行onQueueComplete
                                if (option.onQueueComplete) {
                                    var queueData = uploadManager._allFilesUploaded();
                                    queueData && option.onQueueComplete(queueData);
                                }

                                //清除文件选择框中的已有值
                                _select.val('');
                            }
                        }

                        if (file.status === 0) {
                            file.status = 1;//标记为正在上传
                            option.onUploadStart && option.onUploadStart(file);
                            // 开始上传
                            xhr.open(option.method, option.uploader, true);
                            xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
                            var fd = new FormData();
                            fd.append(option.fileObjName, file);
                            if (option.formData) {
                                for (key in option.formData) {
                                    fd.append(key, option.formData[key]);
                                }
                            }
                            xhr.send(fd);
                        }

                    }
                }
            };

            uploadManager.init();
        });

        return returnObj;
    }
})(jQuery)