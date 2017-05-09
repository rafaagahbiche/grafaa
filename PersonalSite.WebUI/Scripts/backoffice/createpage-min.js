var loadingSelector = 'div.loading';
var tabLinksSelector = 'ul.tab-links';
var confirmSelector = 'div.del-confirm';
var backgroundSelector = 'div.del-background';
var hiddenBlocSelector = 'div.hidden-bloc';

function switchLoading(activate) {
    if (activate) {
        if ($(loadingSelector).hasClass("hide-loading") && !$(loadingSelector).hasClass("show-loading")) {
            $(loadingSelector).removeClass("hide-loading");
            $(loadingSelector).addClass("show-loading");
        }
    } else {
        if (!$(loadingSelector).hasClass("hide-loading") && $(loadingSelector).hasClass("show-loading")) {
            $(loadingSelector).addClass("hide-loading");
            $(loadingSelector).removeClass("show-loading");
        }
    }
}

function disableTabLinks(disable) {
    if (disable) {
        $(tabLinksSelector).children('li').each(function () {
            $(this).find("a").addClass("disabled");
        });
    }
    else {
        $(tabLinksSelector).children('li').each(function () {
            $(this).find("a").removeClass("disabled");
        });
    }
}

function showDeleteWarning(show) {
    if (show) {
        $(confirmSelector).show();
        $(backgroundSelector).show();
    } else {
        $(confirmSelector).hide();
        $(backgroundSelector).hide();
    }
}

function callCreateArticlePageSync(url, pageViewModel) {
    $.ajax({
        url: url,
        cache: false,
        type: "POST",
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(pageViewModel),
        success: function (result, textStatus) {
            if (result !== null && result !== undefined) {
                $('div#pageinfos').html(result.obj);
                disableTabLinks(false);
                switchLoading(false);
            }
        },
        error: function (result) {
            if (result !== null && result !== undefined) {
                $('div#pageinfos').html(result.responseText);
                disableTabLinks(false);
                switchLoading(false);
            }
        }
    });
}


var appendNewTabLink = function () {
    var newTabLink = $("<a href='#tab-1'>New</a>");
    var newLi = $("<li class='tab'></li>");
    $(newLi).append(newTabLink);
    $(tabLinksSelector).children('li.tab').last().after($(newLi).addClass('active'));
}

var initSaveArticlePageEvent = function () {
    $('a.save-page').bind('click', function (e) {
        $(loadingSelector).find('span').html("Saving page content...");
        switchLoading(true);
        var id = $('div#pageinfos').children('input[name="PageId"]').val();
        var parentId = $('div#pageinfos').children('input[name="ParentArticleId"]').val();
        var contentToSend = tinymce.activeEditor.getContent();
        var pageViewModel = { PageId: id, ParentArticleId: parentId, PageContent: $('<div></div>').text(contentToSend).html() };
        callCreateArticlePageSync(this.href, pageViewModel);
        e.preventDefault();
        // return false;
    });
}

var editDeleteEvent = function () {
    $('.delete-page').bind('click', function () {
        disableTabLinks(true);
        showDeleteWarning(true);
    });

    $('.edit-del-cancel').bind('click', function () {
        disableTabLinks(false);
        showDeleteWarning(false);
    });

    $('.edit-del-ok > a').bind('click', function () {
        var idsContainer = $(hiddenBlocSelector).find("div.active").children("input[type='text']");
        var pageId = -1;
        if ($(idsContainer).val() !== "") {
            pageId = $(idsContainer).val();
        }

        showDeleteWarning(false);
        if (pageId !== "-1") {
            $(loadingSelector).find('span').html("Deleting page...");
            switchLoading(true);
            var pageInfo = { id: pageId };
            $.ajax({
                url: $(this).attr('href'),
                cache: false,
                type: "POST",
                data: pageInfo,
                success: function (result, textStatus) {
                    if (result !== null && result !== undefined && result.Status === true) {
                        $(tabLinksSelector).children('li.active').remove();
                        $(hiddenBlocSelector).children('div.active').remove();
                        initFirstTabFirstText(true);
                        initTabNumbers();
                        disableTabLinks(false);
                        switchLoading(false);
                    }
                }
            });
        }
        else {
            $(tabLinksSelector).children('li.active').remove();
            $(hiddenBlocSelector).children('div.active').remove();
            disableTabLinks(false);
            initFirstTabFirstText(true);
            initTabNumbers();
        }
        return false;
    });
}

$(function () {
    function OnSuccessArticlePageSync(id, idsContainer) {
        $(idsContainer).val(id);
        var articleDivId = $(idsContainer).parent().attr("id");
        if (articleDivId === 'tab-1') {
            $(idsContainer).val(id);
            $(idsContainer).parent().attr("id", "tab" + id);
            var activeLink = $(tabLinksSelector).children("li.active").children("a");
            $(activeLink).attr("href", "#tab" + id);
            $(activeLink).html($(tabLinksSelector).children("li").length - 1);
        }
        disableTabLinks(false);
        switchLoading(false);
    }

    var initTabOnClick = function () {
        $(tabLinksSelector).children('li.tab').each(function () {
            $(this).children('a').on('click', function (e) {
                var currentAttrValue = $(this).attr('href');
                var contentText = $(currentAttrValue, hiddenBlocSelector).children('div').html();
                $('textarea.content-edit').val(contentText);
                tinyMCE.activeEditor.setContent(contentText);
                $(currentAttrValue, hiddenBlocSelector).addClass('active').siblings().removeClass('active');
                $(this).parent('li').addClass('active').siblings().removeClass('active');
                e.preventDefault();
            });
        });
    }

    var initTabPlusOnclick = function () {
        $('li.plus a').bind('click', function () {
            var newTabLink = $("<a href='#tab-1'>New</a>");
            $(newTabLink).on('click', function (e) {
                var currentAttrValue = $(this).attr('href');
                var contentText = $(hiddenBlocSelector + currentAttrValue).children('div').html();
                tinyMCE.activeEditor.setContent(contentText);
                $(hiddenBlocSelector + currentAttrValue).addClass('active').siblings().removeClass('active');
                $(this).parent('li').addClass('active').siblings().removeClass('active');
                e.preventDefault();
            });

            var newLi = $("<li class='tab'></li>");
            $(newLi).append(newTabLink);
            $(tabLinksSelector).children('li.active').removeClass('active');
            $(tabLinksSelector).children('li.tab').last().after($(newLi).addClass('active'));
            var baseHtml = "<div id='tab-1' class='active'><div></div><input type='text' value='-1'></input></div>";
            $(hiddenBlocSelector).children('div.active').removeClass('active');
            tinymce.activeEditor.setContent('');
            $(hiddenBlocSelector).append(baseHtml);
        });
    }

    var initEditArticlePageEvent = function() {
        $('div.edit').bind('click', function () {
            var preview = $(this).parent().parent();
            var edit = $(preview).parent().children("div.page-edit");
            $(preview).hide();
            $(edit).show();
        });
    }

    var initTabNumbers = function() {
        var i = 0;
        $(tabLinksSelector).children('li').each(function () {
            var aHtml = $(this).children('a').html();
            if (aHtml !== "New" && aHtml !== "&#x2B;" && aHtml !== "&#x31;" && aHtml !== "+") {
                $(this).children('a').html(i + 1);
            }
            i++;
        });
    }

    var initFirstTabFirstText = function (fillTinyMceEditor) {
        $(hiddenBlocSelector).children('div').first().addClass('active');
        $('li.tab').first().addClass('active');
        var contentToRender = $(hiddenBlocSelector).children('div').first().children("div").html();
        $('textarea.content-edit').val(contentToRender);
        if (fillTinyMceEditor) {
            tinyMCE.activeEditor.setContent(contentToRender);
        }
    }

    var init = function () {
        //loadTinyMce();
        //initFirstTabFirstText(false);
        //initTabOnClick();
        initSaveArticlePageEvent();
        //initEditArticlePageEvent();
        //initTabPlusOnclick();
    }

    init();
});


