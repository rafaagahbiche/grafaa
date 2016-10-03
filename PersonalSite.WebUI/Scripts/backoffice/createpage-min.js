$(function () {
    function loadTinyMce() {
        tinymce.init({
            selector: "textarea.content-edit",
            plugins: [
                "advlist autolink lists link image charmap print preview anchor",
                "searchreplace visualblocks code fullscreen",
                "insertdatetime media textcolor colorpicker table contextmenu paste"
            ],
            toolbar1: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
            toolbar2: "textcolor forecolor backcolor",
            height: 400
        });
    }

    function initTabOnClick() {
        $('ul.tab-links').children('li.tab').each(function () {
            $(this).children('a').on('click', function (e) {
                var currentAttrValue = $(this).attr('href');
                var contentText = $('.hidden-bloc ' + currentAttrValue).children('div').html();
                $('textarea.content-edit').val(contentText);
                tinyMCE.activeEditor.setContent(contentText);
                $('.hidden-bloc ' + currentAttrValue).addClass('active').siblings().removeClass('active');
                $(this).parent('li').addClass('active').siblings().removeClass('active');
                e.preventDefault();
            });
        });
    }

    function initTabPlusOnclick() {
        $('li.plus a').bind('click', function () {
            var newTabLink = $("<a href='#tab-1'>New</a>");
            $(newTabLink).on('click', function (e) {
                var currentAttrValue = $(this).attr('href');
                var contentText = $('.hidden-bloc ' + currentAttrValue).children('div').html();
                tinyMCE.activeEditor.setContent(contentText);
                $('.hidden-bloc ' + currentAttrValue).addClass('active').siblings().removeClass('active');
                $(this).parent('li').addClass('active').siblings().removeClass('active');
                e.preventDefault();
            });
            var newLi = $("<li class='tab'></li>");
            $(newLi).append(newTabLink);
            $('ul.tab-links').children('li.active').removeClass('active');
            $('ul.tab-links').children('li.tab').last().after($(newLi).addClass('active'));
            var baseHtml = "<div id='tab-1' class='active'><div></div><input type='text' value='-1'></input></div>";
            $('div.hidden-bloc').children('div.active').removeClass('active');
            tinymce.activeEditor.setContent('');
            $('div.hidden-bloc').append(baseHtml);
        });
    }

    function switchLoading(activate) {
        if (activate) {
            if ($('div.loading').hasClass("hide-loading") && !$('div.loading').hasClass("show-loading")) {
                $('div.loading').removeClass("hide-loading");
                $('div.loading').addClass("show-loading");
            }
        } else {
            if (!$('div.loading').hasClass("hide-loading") && $('div.loading').hasClass("show-loading")) {
                $('div.loading').addClass("hide-loading");
                $('div.loading').removeClass("show-loading");
            }
        }
    }

    function disableTabLinks(disable) {
        if (disable) {
            $('ul.tab-links').children('li').each(function () {
                $(this).find("a").addClass("disabled");
            });
        }
        else {
            $('ul.tab-links').children('li').each(function () {
                $(this).find("a").removeClass("disabled");
            });
        }
    }

    function showDeleteWarning(show) {
        if (show) {
            $('div.del-confirm').show();
            $('div.del-background').show();
        } else {
            $('div.del-confirm').hide();
            $('div.del-background').hide();
        }
    }

    function OnSuccessArticlePageSync(id, idsContainer) {
        $(idsContainer).val(id);
        var articleDivId = $(idsContainer).parent().attr("id");
        if (articleDivId === 'tab-1') {
            $(idsContainer).val(id);
            $(idsContainer).parent().attr("id", "tab" + id);
            var activeLink = $('ul.tab-links').children("li.active").children("a");
            $(activeLink).attr("href", "#tab" + id);
            $(activeLink).html($('ul.tab-links').children("li").length - 1);
        }
        disableTabLinks(false);
        switchLoading(false);
    }

    function callCreateArticlePageSync(url, pageInfo, idsContainer) {
        $.ajax({
            url: url,
            cache: false,
            type: "POST",
            data: pageInfo,
            success: function (result, textStatus) {
                if (result !== null && result !== undefined) {
                    OnSuccessArticlePageSync(result.Id, idsContainer);
                }
            }
        });
    }

    function initEditArticlePageEvent() {
        $('div.edit').bind('click', function () {
            var preview = $(this).parent().parent();
            var edit = $(preview).parent().children("div.page-edit");
            $(preview).hide();
            $(edit).show();
        });
    }

    function initSaveArticlePageEvent() {
        $('a.save-page').bind('click', function () {
            $('.loading').find('span').html("Saving page content...");
            switchLoading(true);
            var idsContainer = $('.hidden-bloc').find("div.active").children("input[type='text']");
            var pageId = -1;
            if ($(idsContainer).val() !== "") {
                pageId = $(idsContainer).val();
            }

            var articleId = $("#article").children("input[type='hidden']").val();
            var contentToSend = tinymce.activeEditor.getContent();
            $('.hidden-bloc').find("div.active").children("div").html(contentToSend);
            var pageInfo = { id: pageId, articleId: articleId, content: $('<div></div>').text(contentToSend).html() };
            callCreateArticlePageSync($(this).attr('href'), pageInfo, idsContainer);
            return false;
        });

        $('.delete-page').bind('click', function () {
            disableTabLinks(true);
            showDeleteWarning(true);
        });

        $('.edit-del-cancel').bind('click', function () {
            disableTabLinks(false);
            showDeleteWarning(false);
        });

        $('.edit-del-ok > a').bind('click', function () {
            var idsContainer = $('.hidden-bloc').find("div.active").children("input[type='text']");
            var pageId = -1;
            if ($(idsContainer).val() !== "") {
                pageId = $(idsContainer).val();
            }

            showDeleteWarning(false);
            if (pageId !== "-1") {
                $('.loading').find('span').html("Deleting page...");
                switchLoading(true);
                var pageInfo = { id: pageId };
                $.ajax({
                    url: $(this).attr('href'),
                    cache: false,
                    type: "POST",
                    data: pageInfo,
                    success: function (result, textStatus) {
                        if (result !== null && result !== undefined && result.Status === true) {
                            $('ul.tab-links').children('li.active').remove();
                            $('div.hidden-bloc').children('div.active').remove();
                            initFirstTabFirstText(true);
                            initTabNumbers();
                            disableTabLinks(false);
                            switchLoading(false);
                        }
                    }
                });
            }
            else {
                $('ul.tab-links').children('li.active').remove();
                $('div.hidden-bloc').children('div.active').remove();
                disableTabLinks(false);
                initFirstTabFirstText(true);
                initTabNumbers();
            }
            return false;
        });
    }

    function initTabNumbers() {
        var i = 0;
        $('ul.tab-links').children('li').each(function () {
            var aHtml = $(this).children('a').html();
            if (aHtml !== "New" && aHtml !== "&#x2B;" && aHtml !== "&#x31;" && aHtml !== "+") {
                $(this).children('a').html(i + 1);
            }
            i++;
        });
    }

    function initFirstTabFirstText(fillTinyMceEditor) {
        $('div.hidden-bloc').children('div').first().addClass('active');
        $('li.tab').first().addClass('active');
        var contentToRender = $('div.hidden-bloc').children('div').first().children("div").html();
        $('textarea.content-edit').val(contentToRender);
        if (fillTinyMceEditor) {
            tinyMCE.activeEditor.setContent(contentToRender);
        }
    }

    function initAll() {
        loadTinyMce();
        initFirstTabFirstText(false);
        initTabOnClick();
        initSaveArticlePageEvent();
        initEditArticlePageEvent();
        initTabPlusOnclick();
    }

    initAll();
});


function ExecTask(soapReq, headerAction) {
    $.ajax({
        url: webUrl + "/_vti_bin/workflow.asmx",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("SOAPAction", headerAction);
        },
        type: "POST",
        dataType: "xml",
        data: soapReq,
        contentType: "text/xml; charset=\"utf-8\"",
        complete: OnExecWebServiceComlpete,
        success: OnSuccesRetreiveData,
        error: OnFailRetreiveData
    });
}

