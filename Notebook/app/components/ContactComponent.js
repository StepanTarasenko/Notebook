Vue.component('contact', function (resolve) {
    $.get('app/components/ContactComponent.html', function (template) {
        resolve({
            props: ['item'],
            template: template
        })
    })
})