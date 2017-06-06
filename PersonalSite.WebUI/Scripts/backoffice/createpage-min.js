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
    var oldPageId = pageViewModel.PageId;
    $.ajax({
        url: url,
        cache: false,
        type: "POST",
        data: pageViewModel,
        success: function (data) {
            $('div#pageinfos').html(data);
            if (oldPageId == -1) {
                $('li.active').find('a').html($('li.tab').length);
                AssignTabToEditor();
                disableTabLinks(false);
            }

            switchLoading(false);
        },
        error: function (err) {
            var x = 2;
        }
    });
}

var AssignTabToEditor = function () {
    var newPageId = $('div#pageinfos').children('input#editPageId').val();
    var link = $('li.tab').last().children('a')[0].href;
    var oldPageId = link.substring(link.indexOf('=') + 1, link.indexOf('&'));
    if (oldPageId == -1) {
        $('li.tab').last().children('a')[0].href = link.replace(oldPageId, newPageId);
    }
}

var AddTabOnclick = function () {
    $("a#tabplus").bind('click', function (e) {
        $('ul#tabs').children('li.active').removeClass('active');
        disableTabLinks(true);
        var parentId = $('div#article').children('input#pagefuckingId').val();
        $.ajax({
            url: '/Article/AddNewTab',
            type: "GET",
            data: { articleId: parentId },
            success: function (data) {
                $(data).insertBefore('li.plus');
            }
        });

        $.ajax({
            url: '/Article/ShowPageContent',
            type: "GET",
            data: { pageId: -1, articleId: parentId },
            success: function (data) {
                $('div#tobeupdated').html(data);
                initEventsForSelectedTab();
            }
        });

        e.preventDefault();
    });
}

var selectFocusTab = function () {
    $('ul#tabs').children('li.active').removeClass('active');
    $(this).parent().addClass('active');
}

var initEventsForSelectedTab = function () {
    initSavePageEvent();
    editDeleteEvent();
}

var initSavePageEvent = function () {
    $('a.save-page').bind('click', function (e) {
        $(loadingSelector).find('span').html("Saving page content...");
        switchLoading(true);
        var id = $('div#pageinfos').children('input[name="PageId"]').val();
        var parentId = $('div#article').children('input#pagefuckingId').val();
        var contentToSend = tinymce.activeEditor.getContent();
        var pageViewModel = { PageId: id, ParentArticleId: parentId, PageContent: $('<div></div>').text(contentToSend).html() };
        callCreateArticlePageSync(this.href, pageViewModel);
        e.preventDefault();
    });
}

var deleteCurrentTab = function () {
    $('ul#tabs').find('li.active').remove();
    var i = 1;
    $('ul#tabs').children('li.tab').each(function () {
        $(this).find('a').html(i);
        i++;
    });

    $('ul#tabs').children('li:first-child').addClass('active');
}

var turnOffDeleteStyle = function () {
    disableTabLinks(false);
    showDeleteWarning(false);
    deleteCurrentTab();
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
}

$(function () {
    var init = function () {
        AddTabOnclick();
        //loadTinyMce();
        //initFirstTabFirstText(false);
        //initTabOnClick();
        initSavePageEvent();
        editDeleteEvent();
        //initEditArticlePageEvent();
        //initTabPlusOnclick();
    }

    init();
});


