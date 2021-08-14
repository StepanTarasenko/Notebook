Vue.component('contact-label', function (resolve) {
    $.get('app/components/ContactLabel.html', function (template) {
        resolve({
            template: template
        })
    })
})