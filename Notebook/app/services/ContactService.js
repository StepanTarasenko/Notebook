const contactService = (function () {
    const service = {
        get: get,
        add: add,
        remove: remove,
        change: change
    }

    function get() {
        return $.get(`WebService/ContactService.asmx/Get`);
    }

    function add(postData) {
        //return $.post(`WebService/ContactService.asmx/Post`, postData)
        let respons =  $.ajax({
            contentType: 'application/json',
            data: JSON.stringify({ newContact: { ...postData } }),
            processData: false,
            dataType: 'json',
            type: 'POST',
            async: false,
            url: 'WebService/ContactService.asmx/Post'
        });
        //console.log(respons)
        return respons.responseText.split('{')[0];
    }

    function remove(id) {
        return $.get(`WebService/ContactService.asmx/Delete?id=${id}`);
    }

    function change(putData) {
        return $.ajax({
            contentType: 'application/json',
            data: JSON.stringify({ renewContact: { ...putData } }),
            processData: false,
            dataType: 'json',
            type: 'POST',
            url: 'WebService/ContactService.asmx/Put'
        });
    }

    return service
})();