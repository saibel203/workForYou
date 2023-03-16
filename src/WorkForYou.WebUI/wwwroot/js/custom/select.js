$(function () {
    $(".vacancies__searched-bar-sort-select").each(function () {
        $(this).hide();
        let $select = $(this);
        let _id = $(this).attr("id");
        let wrapper = document.createElement("div");
        wrapper.setAttribute("class", "ddl ddl_" + _id);

        let input = document.createElement("input");
        input.setAttribute("type", "text");
        input.setAttribute("class", "ddl-input");
        input.setAttribute("id", "ddl_" + _id);
        input.setAttribute("readonly", "readonly");
        input.setAttribute(
            "placeholder",
            $(this)[0].options[$(this)[0].selectedIndex].innerText
        );

        $(this).before(wrapper);
        let $ddl = $(".ddl_" + _id);
        $ddl.append(input);
        $ddl.append("<div class='ddl-options ddl-options-" + _id + "'></div>");
        let $ddl_input = $("#ddl_" + _id);
        let $ops_list = $(".ddl-options-" + _id);
        let $ops = $(this)[0].options;
        for (let i = 0; i < $ops.length; i++) {
            $ops_list.append(
                "<div data-value='" +
                $ops[i].value +
                "'>" +
                $ops[i].innerText +
                "</div>"
            );
        }

        $ddl_input.click(function () {
            $ddl.toggleClass("active");
        });
        $ddl_input.blur(function () {
            $ddl.removeClass("active");
        });
        $ops_list.find("div").click(function () {
            $select.val($(this).data("value")).trigger("change");
            $ddl_input.val($(this).text());
            $ddl.removeClass("active");
        });
    });

    $('.vacancies__searched-bar-sort form select').change(() => {
        $('.vacancies__searched-bar-sort form').submit();
    });
});
