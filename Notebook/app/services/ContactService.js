const contactService = (function () {
    const service = {
        get: get,
        add: add,
        remove: remove,
        change: change
    }

    function get() {
        return $.get(`WebService/ContactService.asmx/Get`)
    }

    function add(postData) {
        return $.post(`WebService/ContactService.asmx/Post`, postData)
    }

    function remove(id) {
        return $.get(`WebService/ContactService.asmx/Delete?id=${id}`)
    }

    function change(putData) {
        return $.post(`WebService/ContactService.asmx/Put`, putData)
    }

    return service
})();