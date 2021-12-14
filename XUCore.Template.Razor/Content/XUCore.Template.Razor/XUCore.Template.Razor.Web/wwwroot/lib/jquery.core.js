

/* Ajax_upload */
; (function () {

    var d = document, w = window;

    /**
    * Get element by id
    */
    function $(element) {
        if (typeof element == "string")
            element = d.getElementById(element);
        return element;
    }

    /**
    * Attaches event to a dom element
    */
    function addEvent(el, type, fn) {
        if (w.addEventListener) {
            el.addEventListener(type, fn, false);
        } else if (w.attachEvent) {
            var f = function () {
                fn.call(el, w.event);
            };
            el.attachEvent('on' + type, f)
        }
    }


    /**
    * Creates and returns element from html chunk
    */
    var toElement = function () {
        var div = d.createElement('div');
        return function (html) {
            div.innerHTML = html;
            var el = div.childNodes[0];
            div.removeChild(el);
            return el;
        }
    }();

    function hasClass(ele, cls) {
        return ele.className.match(new RegExp('(\\s|^)' + cls + '(\\s|$)'));
    }
    function addClass(ele, cls) {
        if (!hasClass(ele, cls)) ele.className += " " + cls;
    }
    function removeClass(ele, cls) {
        var reg = new RegExp('(\\s|^)' + cls + '(\\s|$)');
        ele.className = ele.className.replace(reg, ' ');
    }

    function getOffset(el) {
        if (w.jQuery) {
            return jQuery(el).offset();
        }

        var top = 0, left = 0;
        do {
            top += el.offsetTop || 0;
            left += el.offsetLeft || 0;
        } while (el = el.offsetParent);

        return {
            left: left,
            top: top
        };
    }

    function getBox(el) {
        var left, right, top, bottom;
        var offset = getOffset(el);
        left = offset.left;
        top = offset.top;
        right = left + el.offsetWidth;
        bottom = top + el.offsetHeight;

        return {
            left: left,
            right: right,
            top: top,
            bottom: bottom
        };
    }

    /**
    * Crossbrowser mouse coordinates
    */
    function getMouseCoords(e) {
        // pageX/Y is not supported in IE
        // http://www.quirksmode.org/dom/w3c_cssom.html			
        if (!e.pageX && e.clientX) {
            return {
                x: e.clientX + d.body.scrollLeft + d.documentElement.scrollLeft,
                y: e.clientY + d.body.scrollTop + d.documentElement.scrollTop
            };
        }

        return {
            x: e.pageX,
            y: e.pageY
        };

    }
    /**
    * Function generates unique id
    */
    var getUID = function () {
        var id = 0;
        return function () {
            return 'ValumsAjaxUpload' + id++;
        }
    }();

    function fileFromPath(file) {
        return file.replace(/.*(\/|\\)/, "");
    }

    function getExt(file) {
        return (/[.]/.exec(file)) ? /[^.]+$/.exec(file.toLowerCase()) : '';
    }

    (function () {
        // iframe will be shared by each instance.
        var iframe = null;

        // Please use AjaxUpload , Ajax_upload will be removed in the next version
        Ajax_upload = AjaxUpload = function (button, options) {
            if (button.jquery) {
                // jquery object was passed
                button = button[0];
            } else if (typeof button == "string" && /^#.*/.test(button)) {
                button = button.slice(1);
            }
            button = $(button);

            this._input = null;
            this._button = button;
            this._disabled = false;
            this._submitting = false;

            this._settings = {
                // Location of the server-side upload script
                action: 'UploadServer.ashx',
                // File upload name
                name: 'userfile',
                // Additional data to send
                data: {},
                // Submit file as soon as it's selected
                autoSubmit: true,
                // When user selects a file, useful with autoSubmit disabled			
                onChange: function (file, extension) { },
                // Callback to fire before file is uploaded
                // You can return false to cancel upload
                onSubmit: function (file, extension) { },
                // Fired when file upload is completed
                onComplete: function (file, response) { }
            };

            // Merge the users options with our defaults
            for (var i in options) {
                this._settings[i] = options[i];
            }

            this._createInput();
            this._rerouteClicks();

            // 1 iframe for all inputs
            if (!iframe) {
                this._createIframe();
            }
        }

        // assigning methods to our class
        AjaxUpload.prototype = {
            setData: function (data) {
                this._settings.data = data;
            },
            setAction: function (url) {
                this._settings.action = url;
            },
            disable: function () {
                this._disabled = true;
            },
            enable: function () {
                this._disabled = false;
            },
            // use setData instead, set_data will be removed in the next version
            set_data: function (data) {
                this.setData(data);
            },
            /**
            * Creates invisible file input above the button 
            */
            _createInput: function () {
                var self = this;

                var input = d.createElement("input");
                input.setAttribute('type', 'file');
                input.setAttribute('name', this._settings.name);
                var styles = {
                    'position': 'absolute'
                    , 'z-index': '999999999999999999'
                    , 'margin': '-5px 0 0 -175px'
                    , 'padding': 0
                    , 'width': '220px'
                    , 'height': '10px'
                    , 'opacity': 0
                    , 'cursor': 'pointer'
                    , 'display': 'none'
                };
                for (var i in styles) {
                    input.style[i] = styles[i];
                }

                // Make sure that element opacity exists
                // (IE uses filter instead)
                if (!(input.style.opacity === "0")) {
                    input.style.filter = "alpha(opacity=0)";
                }
                d.body.appendChild(input);

                addEvent(input, 'change', function () {
                    // get filename from input
                    var file = fileFromPath(this.value);
                    if (self._settings.onChange.call(self, file, getExt(file)) == false) {
                        return;
                    }
                    // Submit form when value is changed
                    if (self._settings.autoSubmit) {
                        self.submit();
                    }
                });

                this._input = input;
            },
            _rerouteClicks: function () {
                var self = this;

                // IE displays 'access denied' error when using this method
                // other browsers just ignore click()
                // addEvent(this._button, 'click', function(e){
                //   self._input.click();
                // });				

                var box, over = false;
                addEvent(self._button, 'mouseover', function (e) {
                    if (!self._input || over) return;
                    over = true;
                    box = getBox(self._button);
                });

                // we can't use mouseout on the button,
                // because invisible input is over it
                addEvent(document, 'mousemove', function (e) {
                    var input = self._input;
                    if (!input || !over) return;
                    if (self._disabled) {
                        removeClass(self._button, 'hover');
                        input.style.display = 'none';
                        return;
                    }

                    var c = getMouseCoords(e);

                    if ((c.x >= box.left) && (c.x <= box.right) &&
                        (c.y >= box.top) && (c.y <= box.bottom)) {

                        input.style.top = c.y + 'px';
                        input.style.left = c.x + 'px';
                        input.style.display = 'block';
                        addClass(self._button, 'hover');
                    } else {
                        // mouse left the button
                        over = false;
                        input.style.display = 'none';
                        removeClass(self._button, 'hover');
                    }
                });

            },
            /**
            * Creates iframe with unique name
            */
            _createIframe: function () {
                // unique name
                // We cannot use getTime, because it sometimes return
                // same value in safari :(
                var id = getUID();
                // I haven't forgotten var statement, 1 iframe will be shared by each instance.
                _iframe = toElement('<iframe name="' + id + '" />');
                _iframe.id = id;
                _iframe.style.display = 'none';
                d.body.appendChild(_iframe);
            },
            /**
            * Upload file without refreshing the page
            */
            submit: function () {
                var self = this, settings = this._settings;

                if (this._input.value === '') {
                    // there is no file
                    return;
                }

                // get filename from input
                var file = fileFromPath(this._input.value);

                // execute user event
                if (!(settings.onSubmit.call(this, file, getExt(file)) == false)) {
                    // Do not submit if user function returns false										
                    var form = this._createForm();
                    form.appendChild(this._input);

                    form.submit();
                    d.body.removeChild(form);
                    form = null;
                    this._input = null;

                    // create new input
                    this._createInput();

                    var iframe = _iframe;
                    addEvent(iframe, 'load', function () {
                        var toDeleteFlag = false;

                        if (iframe.src == "about:blank") {
                            // First time around, do not delete.
                            if (toDeleteFlag) {
                                iframe.remove();
                            }
                            return;
                        }

                        var doc = iframe.contentDocument ? iframe.contentDocument : frames[iframe.id].document;
                        var response = doc.body.innerText;
                        settings.onComplete.call(self, file, response);

                        // Reload blank page, so that reloading main page
                        // does not re-submit the post. Also, remember to
                        // delete the frame
                        iframe.src = "about:blank";
                        toDeleteFlag = true;
                    });

                    // Create new iframe, so we can have multiple uploads at once
                    this._createIframe();

                } else {
                    // clear input to allow user to select same file
                    this._input.value = '';
                }
            },
            /**
            * Creates form, that will be submitted to iframe
            */
            _createForm: function () {
                var settings = this._settings;
                // method, enctype must be specified here
                // because changing this attr on the fly is not allowed in IE 6/7
                // $('<form method="post" enctype="multipart/form-data"></form>')			
                var form = toElement('<form method="post" enctype="multipart/form-data"></form>');
                form.style.display = 'none';
                form.action = settings.action;
                form.target = _iframe.name;
                d.body.appendChild(form);

                // Create hidden input element for each data key
                for (var prop in settings.data) {
                    var el = d.createElement("input");
                    el.type = 'hidden';
                    el.name = prop;
                    el.value = settings.data[prop];
                    form.appendChild(el);
                }
                return form;
            }
        };
    })();
})();

(function ($) {

    /* Ajax_upload ===>>> jQuery('#').bindUpload */
    $.fn.bindUpload = function (opts) {
        opts = $.extend({
            extensions: /^(jpg|png|jpeg|gif)$/,
            extensionmessage: '只能选择jpg|png|jpeg|gif格式的图片',
            server: '/api/upload/image',
            data: 'cutorgin=true&orginsize=600x200',
            success: function (res) { }
        }, opts);

        var bindUpload = function (obj, url, complete, load) {
            try {
                var interval;
                var loading = typeof load == 'undefined' ? null : $(load);
                new Ajax_upload($(obj), {
                    name: 'file',
                    onChange: function (file, extension) {
                        this.setAction(url);
                    },
                    onSubmit: function (file, ext) {
                        if (ext && opts.extensions.test(ext)) {
                            this.disable();
                        } else {
                            new $.flavr({
                                iconPath: '/images/',
                                icon: 'warning.png',
                                title: '上传错误',
                                content: opts.extensionmessage,
                                closeOverlay: true,
                                closeEsc: true
                            });
                            return false;
                        }
                        if (loading != null) {
                            interval = window.setInterval(function () {
                                var text = loading.html();
                                if (text.length < 9)
                                    loading.html(text + '.');
                                else
                                    loading.html('上传中');
                            }, 200);
                        }
                    },
                    onComplete: function (file, response) {
                        if (loading != null) {
                            loading.html('');
                            window.clearInterval(interval);
                        }
                        this.enable();
                        var data = eval('(' + response + ')');
                        if (data.Code === 0) {
                            complete(data);
                        } else {
                            new $.flavr({
                                iconPath: '/images/',
                                icon: 'error.png',
                                title: '上传错误',
                                content: data.message,
                                closeOverlay: true,
                                closeEsc: true
                            });
                        }
                    }
                });
            }
            catch (e) {

            }
        };

        var btnUploader = $(this);
        var btnUploaderText = btnUploader.html();
        bindUpload(btnUploader, opts.server + '?' + opts.data, function (res) {
            btnUploader.html(btnUploaderText);
            opts.success && opts.success(res);
        }, btnUploader);
    }

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $.fn.parsleyForm = function (opts) {
        var _that = $(this);

        opts = $.extend({
            errorsContainer: function (parsleyField) {
                return parsleyField.$element.parent().parent().find('.error');
            },
            validate: function (formInstance) { },
            error: function (formInstance) { },
            submit: function (formData) { }
        }, opts);

        _that.parsley({
            errorsContainer: opts.errorsContainer
        })
            .on('form:validate', opts.validate)
            .on('form:error', opts.error)
            .on('form:submit', function () {
                var formData = _that.serializeObject();
                opts.submit(formData);
                return false;
            });
    };

    $.ajaxSetup({
        //dataType: "json",
        //contentType: "application/json",
        //headers: {
        //    'Content-Type': 'application/x-www-form-urlencoded'
        //},
        error: function (jqXHR, textStatus, errorThrown) {
            switch (jqXHR.status) {
                case (500):
                    show.warning('警告', '服务器系统内部错误');
                    break;
                case (401):
                    show.warning('警告', '未登录');
                    break;
                case (403):
                    show.warning('警告', '无权限执行此操作');
                    break;
                case (408):
                    show.warning('警告', '请求超时');
                    break;
                default:
                    show.warning('警告', '未知错误,请联系管理员');
            }
        },
        cache: false
    });

    $.extend({
        request: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    });

})(jQuery);

var show = {
    warning: function (title, content) {
        new window.parent.$.flavr({
            iconPath: '/images/',
            icon: 'warning.png',
            position: 'center-mid',
            animateEntrance: 'fadeIn',
            title: title,
            content: content,
            closeOverlay: true,
            closeEsc: true
        });
    },
    error: function (title, content) {
        new window.parent.$.flavr({
            iconPath: '/images/',
            icon: 'error.png',
            position: 'center-mid',
            animateEntrance: 'fadeIn',
            title: title,
            content: content,
            closeOverlay: true,
            closeEsc: true
        });
    },
    success: function (title, content) {
        new window.parent.$.flavr({
            iconPath: '/images/',
            icon: 'success.png',
            position: 'center-mid',
            animateEntrance: 'fadeIn',
            title: title,
            content: content,
            autoclose: true,
            closeOverlay: true,
            closeEsc: true,
            timeout: 5000
        });
    },
    successConfirm: function (title, content, buttons) {
        new window.parent.$.flavr({
            iconPath: '/images/',
            icon: 'success.png',
            position: 'center-mid',
            animateEntrance: 'fadeIn',
            title: title,
            content: content,
            closeOverlay: true,
            closeEsc: true,
            buttons: buttons
        });
    },
    confirm: function (confirmHandler, cancelHandler) {
        new window.parent.$.flavr({
            iconPath: '/images/',
            icon: 'warning.png',
            position: 'center-mid',
            animateEntrance: 'fadeIn',
            title: '您确定要进行该操作吗?',
            dialog: 'confirm',
            buttons: {
                confirm: { text: '确认', style: 'danger', action: confirmHandler },
                close: { text: '取消', style: 'default', action: cancelHandler }
            }
        });
    },
    confirmWarn: function (title, confirmHandler, cancelHandler) {
        new window.parent.$.flavr({
            iconPath: '/images/',
            icon: 'warning.png',
            position: 'center-mid',
            animateEntrance: 'fadeIn',
            title: title,
            dialog: 'confirm',
            buttons: {
                confirm: { text: '确认', style: 'danger', action: confirmHandler },
                close: { text: '取消', style: 'default', action: cancelHandler }
            }
        });
    }
}

var createTable = function ($table, opts) {
    if (opts.defaultColumn && opts.defaultColumn.operate) {
        opts.columns.unshift({ field: 'operate', title: '操作', align: 'center', width: 40, formatter: operateFormatter });
    }
    if (opts.defaultColumn && opts.defaultColumn.check) {
        opts.columns.unshift({ field: 'state', checkbox: true, align: 'center', valign: 'middle' });
    }
    if (opts.defaultColumn && opts.defaultColumn.weight) {
        opts.columns.push({
            field: 'Weight', title: '权重', sortable: true, align: 'center', width: 40,
            editable: {
                type: 'text', title: '权重', placement: 'left', validate: function (value) {
                    value = $.trim(value);
                    if (value) {
                        if (!/^([1-9]\d*|[0]{1,1})$/.test(value))
                            return '只能输入整数!';
                    }
                }
            }
        });
    }
    if (opts.defaultColumn && opts.defaultColumn.status) {
        opts.columns.push({ field: 'Status', title: '状态', width: 40, sortable: true, align: 'center', formatter: statusFormatter });
    }
    if (opts.defaultColumn && opts.defaultColumn.create) {
        opts.columns.push({ field: 'CreatedAtUserName', title: '发布人', width: 140, sortable: true, align: 'center' });
        opts.columns.push({ field: 'CreatedAt', title: '发布日期', width: 140, sortable: true, align: 'center', formatter: formatterTime });
    }
    if (opts.defaultcolumn && opts.defaultColumn.update) {
        opts.columns.push({ field: 'ModifiedAtUserName', title: '修改人', sortable: true, align: 'center' });
        opts.columns.push({ field: 'ModifiedAt', title: '修改日期', width: 140, sortable: true, align: 'center', formatter: formatterTime });
    }
    opts = $.extend({
        classes: 'table table-striped b-t b-light',
        clickToSelect: false,
        mobileResponsive: true,
        search: false,
        pagination: true,
        showHeader: true,
        showColumns: false,
        showRefresh: false,
        detailView: false,
        detailFormatter: function () { },
        sortName: "Id",
        sortOrder: "desc",
        toolbar: '#custom-toolbar',
        toolbarAlign: '',
        idField: 'Id',
        url: '/data.ashx',
        queryParams: function (params) { },
        sidePagination: 'server',
        pageSize: 15,
        pageList: [10, 15, 25, 50, 100, 150],
        columns: [],
        iconSize: 'sm',
        iconsPrefix: 'fa',
        icons: {
            paginationSwitchDown: 'fa-chevron-down',
            paginationSwitchUp: 'fa-chevron-up',
            refresh: 'fa-refresh',
            toggle: 'fa-list',
            columns: 'fa-th',
            detailOpen: 'fa-plus-circle',
            detailClose: 'fa-minus-circle'
        },
        contextMenu: '#context-menu',
        contextMenuButton: '.operate-button',
        onContextMenuItem: function () { },
        onEditableSave: function () { }
    }, opts);
    return $table.bootstrapTable(opts);
}
var createSimpleTable = function ($table, opts) {
    if (opts.defaultColumn && opts.defaultColumn.operate) {
        opts.columns.unshift({ field: 'operate', title: '操作', align: 'center', width: 40, formatter: operateFormatter });
    }
    if (opts.defaultColumn && opts.defaultColumn.check) {
        opts.columns.unshift({ field: 'state', checkbox: true, align: 'center', valign: 'middle' });
    }
    if (opts.defaultColumn && opts.defaultColumn.weight) {
        opts.columns.push({
            field: 'Weight', title: '权重', sortable: true, align: 'center', width: 40,
            editable: {
                type: 'text', title: '权重', placement: 'left', validate: function (value) {
                    value = $.trim(value);
                    if (value) {
                        if (!/^([1-9]\d*|[0]{1,1})$/.test(value))
                            return '只能输入整数!';
                    }
                }
            }
        });
    }
    if (opts.defaultColumn && opts.defaultColumn.status) {
        opts.columns.push({ field: 'Status', title: '状态', width: 40, sortable: true, align: 'center', formatter: statusFormatter });
    }
    if (opts.defaultColumn && opts.defaultColumn.create) {
        opts.columns.push({ field: 'CreatedAtUserName', title: '发布人', width: 140, sortable: true, align: 'center' });
        opts.columns.push({ field: 'CreatedAt', title: '发布日期', width: 140, sortable: true, align: 'center', formatter: formatterTime });
    }
    if (opts.defaultcolumn && opts.defaultColumn.update) {
        opts.columns.push({ field: 'ModifiedAtUserName', title: '修改人', sortable: true, align: 'center' });
        opts.columns.push({ field: 'ModifiedAt', title: '修改日期', width: 140, sortable: true, align: 'center', formatter: formatterTime });
    }
    opts = $.extend({
        classes: 'table table-striped b-t b-light',
        clickToSelect: true,
        mobileResponsive: true,
        search: false,
        pagination: false,
        showHeader: false,
        showColumns: false,
        showRefresh: false,
        detailView: false,
        detailFormatter: function () { },
        idField: 'Id',
        url: '/data',
        queryParams: function (params) { },
        sidePagination: 'server',
        pageSize: 15,
        pageList: [10, 15, 25, 50, 100, 150],
        columns: [],
        iconSize: 'sm',
        iconsPrefix: 'fa',
        icons: {
            paginationSwitchDown: 'fa-chevron-down',
            paginationSwitchUp: 'fa-chevron-up',
            refresh: 'fa-refresh',
            toggle: 'fa-list',
            columns: 'fa-th',
            detailOpen: 'fa-plus-circle',
            detailClose: 'fa-minus-circle'
        }
    }, opts);
    return $table.bootstrapTable(opts);
}


var getSelections = function ($table) {
    return $.map($table.bootstrapTable('getSelections'), function (row) {
        return row.Id
    });
}
var operateFormatter = function (value, row) {
    var html = '';
    html += '<div class="btn-group btn-group-xs">';
    html += '   <button type="button" class="btn btn-default dropdown-toggle operate-button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">';
    html += '       <span class="caret"></span>';
    html += '       <span class="sr-only">Toggle Dropdown</span>';
    html += '   </button>';
    html += '</div>';
    return html;
}
var operateBoolFormatter = function (value, row) {
    var html = '';
    if (value)
        html += '<i class="fa fa-check text-success"></i>';
    else
        html += '-';
    return html;
}
var statusFormatter = function (value, row) {
    if (row.Status == 1) return '<i class="fa fa-check text-success"></i>';
    else if (row.Status == 2) return '<i class="fa fa-eye-slash text-dark"></i>';
    else if (row.Status == 3) return '<i class="fa fa-trash-o text-dark"></i>';
    else return '';
}

var statusModel = {
    show: 1,
    soldout: 2,
    trash: 3
}


//var __formatterDate = function (date, istime) {
//    function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
//    if (!istime) {
//        var ar_date = [date.getFullYear(), date.getMonth() + 1, date.getDate()];
//        for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
//        return ar_date.join('-');
//    } else {
//        var ar_date = [date.getFullYear(), date.getMonth() + 1, date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()];
//        for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
//        return ar_date.slice(0, 3).join('-') + ' ' + ar_date.slice(3).join(':');
//    }
//};
//var formatterDate = function (strdate) {
//    var date = StringToDate(strdate);
//    if (date == null) return '';
//    return __formatterDate(date);
//}
//function StringToDate(str) { //字符串转为日期
//    if (typeof str == 'string') {
//        var results = str.match(/^ *(\d{4})-(\d{1,2})-(\d{1,2}) *$/);
//        if (results && results.length > 3)
//            return new Date(results[1], results[2] - 1, results[3]);
//        results = str.match(/^ *(\d{4})-(\d{1,2})-(\d{1,2}) +(\d{1,2}):(\d{1,2}):(\d{1,2}) *$/);
//        if (results && results.length > 6)
//            return new Date(results[1], results[2] - 1, results[3], results[4], results[5], results[6]);
//        results = str.match(/^ *(\d{4})-(\d{1,2})-(\d{1,2}) +(\d{1,2}):(\d{1,2}):(\d{1,2})\.(\d{1,9}) *$/);
//        if (results && results.length > 7)
//            return new Date(results[1], results[2] - 1, results[3], results[4], results[5], results[6], results[7]);
//    }
//    return str;
//}
var formatterTime = function (val, fmt) {
    if (val == null) return '-';
    fmt = typeof fmt !== 'string' ? 'YYYY-MM-dd hh:mm:ss' : fmt;
    if (typeof val === 'string') return dateFormat(fmt, new Date(val));
    return dateFormat(fmt, val);
}
//生成随机 GUID 数
var guid = function () {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}
var dateFormat = function (fmt, date) {
    let ret;
    const opt = {
        "Y+": date.getFullYear().toString(),        // 年
        "M+": (date.getMonth() + 1).toString(),     // 月
        "d+": date.getDate().toString(),            // 日
        "h+": date.getHours().toString(),           // 时
        "m+": date.getMinutes().toString(),         // 分
        "s+": date.getSeconds().toString()          // 秒
        // 有其他格式化字符需求可以继续添加，必须转化成字符串
    };
    for (let k in opt) {
        ret = new RegExp("(" + k + ")").exec(fmt);
        if (ret) {
            fmt = fmt.replace(ret[1], (ret[1].length == 1) ? (opt[k]) : (opt[k].padStart(ret[1].length, "0")))
        };
    };
    return fmt;
}
//字符串转换为时间戳
function getDateTimeStamp(dateStr) {
    //return Date.parse(dateStr.replace(/-/gi, "/"));
    return Date.parse(dateStr);
}
function getDateDiff(dateStr) {
    var publishTime = getDateTimeStamp(dateStr) / 1000,
        d_seconds,
        d_minutes,
        d_hours,
        d_days,
        timeNow = parseInt(new Date().getTime() / 1000),
        d,

        date = new Date(publishTime * 1000),
        Y = date.getFullYear(),
        M = date.getMonth() + 1,
        D = date.getDate(),
        H = date.getHours(),
        m = date.getMinutes(),
        s = date.getSeconds();
    //小于10的在前面补0
    if (M < 10) {
        M = '0' + M;
    }
    if (D < 10) {
        D = '0' + D;
    }
    if (H < 10) {
        H = '0' + H;
    }
    if (m < 10) {
        m = '0' + m;
    }
    if (s < 10) {
        s = '0' + s;
    }

    d = timeNow - publishTime;
    d_days = parseInt(d / 86400);
    d_hours = parseInt(d / 3600);
    d_minutes = parseInt(d / 60);
    d_seconds = parseInt(d);

    if (d_days >= 30)
        return Y + '-' + M + '-' + D + '&nbsp;' + H + ':' + m;
    else if (d_days >= 14)
        return "2周前";
    else if (d_days >= 7)
        return "1周前";
    else if (d_days >= 1)
        return d_days + "天前";
    else if (d_days <= 0 && d_hours > 0)
        return d_hours + '小时前';
    else if (d_hours <= 0 && d_minutes > 0)
        return d_minutes + '分钟前';
    else if (d_seconds < 60) {
        if (d_seconds <= 0)
            return '刚刚';
        else
            return d_seconds + '秒前';
    }
}


var csrfSafeMethod = function (method) {
    // these HTTP methods do not require CSRF protection
    return (/^(GET|HEAD|OPTIONS|TRACE)$/.test(method));
}
$.ajaxSetup({
    beforeSend: function (xhr, settings) {
        if (!csrfSafeMethod(settings.type) && !this.crossDomain) {
            xhr.setRequestHeader("RequestVerificationToken", $('input:hidden[name="__RequestVerificationToken"]').val());
        }
    }
});