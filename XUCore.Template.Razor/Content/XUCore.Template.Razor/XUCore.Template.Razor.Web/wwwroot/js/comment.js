var reply = function (commentId, commentReplyId, name) {
    $('.comment-list #data-form').data('CommentId', commentId);
    $('.comment-list #data-form').data('CommentReplyId', commentReplyId);
    $('.comment-list #data-form').data('Name', name);
}
$(function () {
    var replyTmpl = function (row) {
        var html = [];
        html.push('<article class="comment-item comment-reply" data-replycommentid="' + row.CommentId + '">');
        html.push('     <a class="pull-left thumb-sm avatar">');
        html.push('        <img src="/images/a8.png" alt="...">');
        html.push('     </a>');
        html.push('     <span class="arrow left"></span>');
        html.push('     <section class="comment-body panel panel-default">');
        html.push('         <div class="panel-body">');
        html.push('             <span class="text-muted m-l-sm pull-right"><i class="fa fa-clock-o m-r-xs"></i>' + getDateDiff(row.ReplyTime.replace("T", " ")) + '</span>');
        if (row.CommentReplyId > 0)
            html.push('             <a href="#comment-form" class="text-info" onclick="reply(' + row.CommentId + ',' + row.Id + ',\'' + row.Name + '\');">' + row.Name + '</a> 回复 <a href="#comment-form" class="text-info" onclick="reply(' + row.CommentId + ',' + row.CommentReplyId + ',\'' + row.ReplyName + '\');">' + row.ReplyName + '</a>：' + row.ReplyMessage);
        else
            html.push('             <a href="#comment-form" class="text-info" onclick="reply(' + row.CommentId + ',' + row.Id + ',\'' + row.Name + '\');">' + row.Name + '</a>：' + row.ReplyMessage);
        html.push('         </div>');
        html.push('     </section>');
        html.push('</article>');

        return html.join('');
    }
    var tmpl = function (row) {
        var html = [];
        html.push('<article class="comment-item" data-commentid="' + row.Id + '" data-commentcount="' + row.ReplyCount + '">');
        html.push('    <a class="pull-left thumb-sm avatar">');
        html.push('        <img src="/images/a1.png" class="img-circle" alt="...">');
        html.push('     </a>');
        html.push('     <span class="arrow left"></span>');
        html.push('        <section class="comment-body panel panel-default">');
        html.push('         <header class="panel-heading bg-white">');
        html.push('             <a href="#" class="font-bold">' + row.Name + '</a>');
        html.push('             <span class="text-muted m-l-sm pull-right"><i class="fa fa-clock-o m-r-xs"></i>' + getDateDiff(row.ReplyTime.replace("T", " ")) + '</span>');
        html.push('         </header>');
        html.push('         <div class="panel-body">');
        html.push('             <div>' + row.ReplyMessage + '</div>');
        html.push('             <div class="comment-action m-t-sm">');
        html.push('                 <a href="#comment-form" onclick="reply(' + row.Id + ',0,\'' + row.Name + '\');" class="btn btn-xs text-muted"><i class="fa fa-mail-reply"></i></a>');
        if (row.ReplyCount > 0)
            html.push('                 <a href="javascript:;" class="m-l-sm"><i class="fa fa-comment-o icon-muted"></i> ' + row.ReplyCount + '</a>');
        html.push('             </div>');
        html.push('         </div>');
        html.push('     </section>');
        html.push('</article>');
        return html.join('');
    }
    var bindReply = function (all, commentid, replycount) {
        $.get('/api/articlecomment/tolistcommentreply' + all,
            {
                commentid: commentid
            },
            function (replys) {
                var html = '';
                for (var ndx = 0; ndx < replys.data.rows.length; ndx++) {
                    html += replyTmpl(replys.data.rows[ndx]);
                }
                if (all.length == 0 && replycount > 3) {
                    html += '<article class="comment-item m-b" data-replycommentid="' + commentid + '">';
                    html += '    <section class="comment-body text-center">';
                    html += '        <a href="javascript:;" class="text-xs" rel="viewall" data-commentid="' + commentid + '">查看全部(' + replycount + ')</a>';
                    html += '    </section>';
                    html += '</article>';
                }
                $('article[data-replycommentid=' + commentid + ']').remove();
                $('article[data-commentid=' + commentid + ']').after(html);
                $('a[rel=viewall][data-commentid=' + commentid + ']').click(function () {
                    bindReply('all', $(this).data('commentid'));
                });
            });
    }
    var bindData = function (number) {
        $.get('/api/articlecomment/topagecomment',
            {
                pageNumber: number,
                articleid: articleid
            },
            function (res) {
                var html = '';
                for (var ndx = 0; ndx < res.Data.rows.length; ndx++) {
                    html += tmpl(res.Data.rows[ndx]);
                }
                $('#commentlist').html(html);

                $('article[data-commentid]').each(function () {
                    var count = $(this).data('commentcount');
                    if (count > 0) bindReply('', $(this).data('commentid'), count);
                });
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

    $('.comment-list #data-form').parsleyForm({
        submit: function (formData) {
            formData['ArticleId'] = articleid;
            formData['CommentId'] = $('.comment-list #data-form').data('CommentId') || 0;
            formData['CommentReplyId'] = $('.comment-list #data-form').data('CommentReplyId') || 0;
            formData['ReplyName'] = $('.comment-list #data-form').data('Name') || '';
            formData['ReplyMessage'] = formData['ReplyMessage'].replace(/\r\n/g, '<br/>').replace(/\n/g, '<br/>').replace(/\s/g, ' ');

            $.ajax({
                type: 'post',
                url: '/api/articlecomment/postcomment',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                data: JSON.stringify(formData),
                success: function (result) {
                    if (result.Code == 0) {
                        show.success('', result.Message);
                        if (formData['CommentId'] > 0) {
                            bindReply('all', formData['CommentId'])
                        }
                        if (formData['CommentId'] == 0 && formData['CommentReplyId'] == 0) {
                            bindData(1);
                        }
                        $('.comment-list #data-form').data('CommentId', 0);
                        $('.comment-list #data-form').data('CommentReplyId', 0);
                        $('.comment-list textarea[name=ReplyMessage]').val('');
                    } else {
                        show.error('操作提示', result.Message);
                    }
                }
            });
        }
    });
});