$(function () {
    function loadTinyMce() {
        tinymce.init({
            selector: "textarea.content-edit",
            plugins: [
                "advlist autolink lists link image charmap print preview anchor",
                "searchreplace visualblocks code fullscreen",
                "insertdatetime media table contextmenu paste"
            ],
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
            height : 400
        });
    }

    loadTinyMce();
    $('.article-page').first().addClass('active');

    $('li.tab').first().addClass('active');

    $('.tabs li.tab a').on('click', function (e) {
        var currentAttrValue = $(this).attr('href');
        $('.tab-content ' + currentAttrValue).addClass('active').siblings().removeClass('active');
        $(this).parent('li').addClass('active').siblings().removeClass('active');
        e.preventDefault();
    });

    function initTabPlusOnclick() {
        $('li.plus a').bind('click', function () {
            var newTabLink = $("<a href='#tab-1'>New</a>");
            $(newTabLink).on('click', function (e) {
                var currentAttrValue = $(this).attr('href');
                $('.tab-content ' + currentAttrValue).addClass('active').siblings().removeClass('active');
                $(this).parent('li').addClass('active').siblings().removeClass('active');
                e.preventDefault();
            });

            var newLi = $("<li class='tab'></li>");
            $(newLi).append(newTabLink);
            $('ul.tab-links').children('li.active').removeClass('active');
            $('ul.tab-links').children('li.tab').last().after($(newLi).addClass('active'));
            removeMceEditorToTextarea();
            var baseHtml = $('div.tab-content').html();
            //$.post("/Article/DisplayEmptyCreateArticlePage/").done(function (response) {
            //    $('div.tab-content').append(response);
            //    initSaveArticlePageEvent();
            //    initEditArticlePageEvent();
            //    initTabPlusOnclick()
            //    $(response).children("div.active").removeClass('active');
            //    $(response).children('div').last().addClass('active');
            //                loadTinyMce();
            //    $('div.tab-content').children('div').each(function () {
            //        var s = $(this).find('textarea').attr('id');
            //        tinyMCE.execCommand('mceAddEditor', false, s);
            //    });
            //});

            $('div.tab-content').load("/Article/DisplayEmptyCreateArticlePage/", function (response, status, xhr) {
                $(this).prepend(baseHtml);
                addMceEditorToTextarea();
                initSaveArticlePageEvent();
                initEditArticlePageEvent();
                initTabPlusOnclick()
                $(this).children('div').each(function () {
                    $(this).find('a.save-page').on('click', function () {
                        //                    saveArticlePage($(this));
                        var loading = $(this).parent().parent().parent().children("div.loading");
                        var previewBloc = $(this).parent().parent().parent().children("div.page-preview");
                        var previewContent = $(previewBloc).children("div.html-content");
                        var edit = $(this).parent().parent();
                        switchLoading(loading, true);
                        var idsContainer = $(this).parents('.article-page').find("input[type='hidden']");
                        var contentToSend = tinymce.activeEditor.getContent();
                        var textareaOrigin = $(this).parent().parent().children("textarea");
                        var pageId = -1;

                        if ($(idsContainer).val() !== "") {
                            pageId = $(idsContainer).val();
                        }
                        var articleId = $("#article").children("input[type='hidden']").val();
                        var pageInfo = { id: pageId, articleId: articleId, content: $('<div></div>').text(contentToSend).html() };
                        callCreateArticlePageSync($(this).attr('href'), pageInfo, idsContainer, loading, previewContent, edit, previewBloc, textareaOrigin);
                        return false;
                    });
                    $(this).find('div.edit').on('click', function () {
                        editArticlePage($(this));
                    });
                });
                $(this).children("div.active").removeClass('active');
                $(this).children('div').last().addClass('active');
            });
        });
    }

    function switchLoading(loading, activate) {
        if (activate) {
            if ($(loading).hasClass("hide-loading") && !$(loading).hasClass("show-loading")) {
                $(loading).removeClass("hide-loading");
                $(loading).addClass("show-loading");
            }
        } else {
            if (!$(loading).hasClass("hide-loading") && $(loading).hasClass("show-loading")) {
                $(loading).addClass("hide-loading");
                $(loading).removeClass("show-loading");
            }
        }
    }

    function OnSuccessArticlePageSync(id, content, idsContainer, loading, edit, previewBloc, previewContent, textareaOrigin) {
        $(idsContainer).val(id);
        var articleDivId = $(idsContainer).parent().attr("id");
        if (articleDivId === 'tab-1') {
            $(idsContainer).parent().attr("id", "tab" + id);
            var activeLink = $('ul.tab-links').children("li.active").children("a");
            $(activeLink).attr("href", "#tab" + id);
            $(activeLink).html($('ul.tab-links').children("li").length - 1);
        }
        $(textareaOrigin).attr('id', "pageContent" + id);
        $(textareaOrigin).attr('name', "pageContent" + id);
        switchLoading(loading, false);
        $(previewContent).html(content);
        $(edit).hide();
        $(previewBloc).show();
        removeMceEditorToTextarea();
        addMceEditorToTextarea();
    }

    function callCreateArticlePageSync(url, pageInfo, idsContainer, loading, previewContent, edit, previewBloc, textareaOrigin) {
        $.ajax({
            url: url,
            cache: false,
            type: "POST",
            data: pageInfo,
            success: function (result, textStatus) {
                if (result !== null && result !== undefined) {
                    OnSuccessArticlePageSync(result.Id, result.Content, idsContainer, loading, edit, previewBloc, previewContent, textareaOrigin);
                }
            }
        });
    }

    function editArticlePage(button) {
        var preview = $(button).parent().parent();
        var edit = $(preview).parent().children("div.page-edit");
        $(preview).hide();
        $(edit).show();
    }

    function initEditArticlePageEvent() {
        $('div.edit').bind('click', function () {
            editArticlePage($(this));
        });
    }

    function saveArticlePage(button) {
        var loading = $(button).parent().parent().parent().children("div.loading");
        var previewBloc = $(button).parent().parent().parent().children("div.page-preview");
        var previewContent = $(previewBloc).children("div.html-content");
        var edit = $(button).parent().parent();
        switchLoading(loading, true);
        var idsContainer = $(button).parents('.article-page').find("input[type='hidden']");
        var contentToSend = tinymce.activeEditor.getContent();
        var textareaOrigin = $(this).parent().parent().children("textarea");
        var pageId = -1;
        if ($(idsContainer).val() !== "") {
            pageId = $(idsContainer).val();
        }

        var articleId = $("#article").children("input[type='hidden']").val();
        var pageInfo = { id: pageId, articleId:articleId, content: $('<div></div>').text(contentToSend).html() };
        callCreateArticlePageSync($(button).attr('href'), pageInfo, idsContainer, loading, previewContent, edit, previewBloc);
        return false;
    }

    function initSaveArticlePageEvent() {
        $('a.save-page').bind('click', function () {
            //        saveArticlePage($(this));
            var loading = $(this).parent().parent().parent().children("div.loading");
            var previewBloc = $(this).parent().parent().parent().children("div.page-preview");
            var previewContent = $(previewBloc).children("div.html-content");
            var edit = $(this).parent().parent();
            switchLoading(loading, true);
            var idsContainer = $(this).parents('.article-page').find("input[type='hidden']");
            var contentToSend = tinymce.activeEditor.getContent();
            var textareaOrigin = $(this).parent().parent().children("textarea");
            var pageId = -1;
            if ($(idsContainer).val() !== "") {
                pageId = $(idsContainer).val();
            }
            var articleId = $("#article").children("input[type='hidden']").val();
            var pageInfo = { id: pageId, articleId: articleId, content: $('<div></div>').text(contentToSend).html() };
            callCreateArticlePageSync($(this).attr('href'), pageInfo, idsContainer, loading, previewContent, edit, previewBloc, textareaOrigin);
            return false;
        });
    }

    function addMceEditorToTextarea() {
        $('div.tab-content').children('div').each(function () {
            var s = $(this).find('textarea').attr('id');
            tinyMCE.execCommand('mceAddEditor', false, s);
        });
    }

    function removeMceEditorToTextarea() {
        $('div.tab-content').children('div').each(function () {
            var s = $(this).find('textarea').attr('id');
            tinyMCE.execCommand('mceRemoveEditor', false, s);
        });
    }

    initSaveArticlePageEvent();
    initEditArticlePageEvent();
    initTabPlusOnclick();
});
