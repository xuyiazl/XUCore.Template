
$(function () {
    var tmpl = function (row) {
        var html = [];
        html.push('<article class="note-item">');
        html.push('    <a class="pull-left thumb-sm avatar">');
        html.push('        <img src="/images/a1.png" class="img-circle" alt="...">');
        html.push('     </a>');
        html.push('     <span class="arrow left"></span>');
        html.push('        <section class="note-body panel panel-default">');
        html.push('         <header class="panel-heading bg-white">');
        html.push('             <a href="#" class="font-bold">' + row.Name + '</a>');
        html.push('             <span class="text-muted m-l-sm pull-right"><i class="fa fa-clock-o m-r-xs"></i>' + getDateDiff(row.Created_At.replace("T", " ")) + '</span>');
        html.push('         </header>');
        html.push('         <div class="panel-body" style="padding:0px;">');
        html.push('             <div class="md-note-content-view" id="md-note-content-view-' + row.Id + '" style="padding:15px;"><textarea style="display:none;">' + row.Contents + '</textarea></div>');
        html.push('         </div>');
        html.push('     </section>');
        html.push('</article>');
        return html.join('');
    }
    var bindData = function (number) {
        $.get('/api/articlenote/topagenote',
            {
                pageNumber: number,
                articleid: articleid
            },
            function (res) {
                var html = '';
                for (var ndx = 0; ndx < res.Data.rows.length; ndx++) {
                    html += tmpl(res.Data.rows[ndx]);
                }
                $('#notelist').html(html);

                $(".md-note-content-view").each(function () { editormdPreviewInit($(this).attr('id')); });

                if (res.Data.pages > 1) {
                    $('#pagination').parent().parent().show();
                    $('#pagination').jqPaginator('option', {
                        currentPage: res.Data.number,
                        totalPages: res.Data.pages,
                    });
                } else {
                    $('#pagination').parent().parent().hide();
                }
            });
    }
    $.jqPaginator('#pagination', {
        currentPage: 1,
        totalPages: 1,
        visiblePages: 8,
        prev: '<li class="prev"><a href="javascript:;"><i class="fa fa-angle-left"></i></a></li>',
        next: '<li class="next"><a href="javascript:;"><i class="fa fa-angle-right"></i></a></li>',
        page: '<li class="page"><a href="javascript:;">{{page}}</a></li>',
        onPageChange: function (num, type) {
            if (type != 'init') bindData(num);
        }
    });

    bindData(1);

    var editor = editormdSimpleInit('Contents');

    $('.note-list #data-form').parsleyForm({
        submit: function (formData) {
            formData['ArticleId'] = articleid;

            formData['Contents'] = editor.getMarkdown();

            if (formData['Contents'].length == 0) {
                show.error('操作提示', '如果笔记内容都不填写，那叫什么笔记呢？');
                return;
            }

            $.ajax({
                type: 'post',
                url: '/api/articlenote/postnote',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                data: JSON.stringify(formData),
                success: function (result) {
                    if (result.Code == 0) {
                        show.success('', result.Message);
                        bindData(1);
                        editor.setMarkdown('')
                    } else {
                        show.error('操作提示', result.Message);
                    }
                }
            })
        }
    });
});