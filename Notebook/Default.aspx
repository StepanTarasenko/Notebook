<%@ Page Title="NodeBook" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Notebook._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        li {
            list-style: none;
        }
    </style>
    <script src="app/services/contactService.js"></script>
    <script src="app/components/ContactComponent.js"></script>
    <script src="app/components/PopUpDialog.js"></script>

    <div id="app">
        <input type="button" value="show" @click="displayPopUpAdd" />

        <popup-dialog v-bind:show.sync="isDisplayPopUpAdd">
            <input type="text" name="newContact.name" v-model="newContact.name" />
            <input type="button" value="add" @click="addContact" />
        </popup-dialog>

        <popup-dialog v-bind:show.sync="isDisplayPopUpChange">
            <input type="text" name="sel.name" v-model="sel.name" />
            <input type="button" value="change" @click="changeContact(sel)" />
        </popup-dialog>

        <ul>
            <li v-for="item in contacts" :key="item.id">
                <input v-if="sel?.id == item?.id" type="button" :value="`remove${item.id}`" @click="removeContact(item)" />  
                <input v-if="sel?.id == item?.id" type="button" :value="`change${item.id}`" @click="displayPopUpChange" />  
                <contact @click.native="sel = {...item}" :item="item" ></contact>
            </li>
        </ul>
    </div>

    <script>

        let app = new Vue({
            el: '#app',
            data: {
                newContact: {
                    name: ''
                },
                contacts: [],
                sel: {
                    name: String,
                    id: Number
                },
                isDisplayPopUpAdd: false,
                isDisplayPopUpChange: false
            },

            methods: {
                getContacts: function () {
                    contactService.get().then(contacts => this.contacts = contacts) 
                },
                addContact: function () { 
                    let contact = {
                        id: 0,
                        name: this.newContact.name
                    }
                    contactService.add(contact).then(id => contact.id = id) 
                    this.contacts.push(contact)
                    this.newContactsName = null
                    this.isDisplayPopUpAdd = false;
                },
                removeContact: function (item) {
                    contactService.remove(item.id).then(x => {
                        this.contacts = this.contacts.filter(contact => contact !== item)
                    })
                },
                changeContact: function (item) {
                    let contact = this.contacts.find(element => element.id == item.id)
                     contact.name = item.name
                    contactService.change(contact).then(
                        this.contacts = this.contacts.map(item => item.id == contact.if ? contact : item)
                    )
                    this.isDisplayPopUpChange = false;
                },
                displayPopUpAdd: function () {
                    this.isDisplayPopUpAdd = true;
                },
                displayPopUpChange: function () {
                    this.isDisplayPopUpChange = true;
                }


            },

            created: function () { 
                this.getContacts()
            }
        })
    </script>
   
    
</asp:Content>
