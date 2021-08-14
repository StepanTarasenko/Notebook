<%@ Page Title="NodeBook" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Notebook._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .contacts-box li {
            list-style: none;
            display: flex;
        }
        .contacts-box li div {
            display: inline-block;
        }
        label {
            display: inline-block;
            width: 125px;
        }


        .id {
            width: 40px;
        }
        .button-container,
        .text {
            width: 100px;
        }
        .email,
        .date {
            width:150px;
        }        

    </style>
    <script src="app/services/contactService.js"></script>
    <script src="app/components/ContactComponent.js"></script>
    <script src="app/components/ContactLabel.js"></script>
    <script src="app/components/PopUpDialog.js"></script>
    <div id="app">          
            <button type="button" @click="displayPopUpAdd">
                <i class="fa fa-plus" aria-hidden="true" ></i>
            </button>


        <!--Change-->
        <!--fail to create a separate component-->
        <popup-dialog v-bind:show.sync="isDisplayPopUpChange">

            <p v-if="errors.length">
                <b>Errors:</b>
                <ul>
                    <li v-for="error in errors">{{ error }}</li>
                </ul>
            </p>

            <p>
                <label>Name</label>
                <input type="text" name="sel.name" v-model="sel.name" />
            </p>
            
            <p>
                <label>SurName</label>
                <input type="text" name="sel.surName" v-model="sel.surName" />          
            </p>

            <p>
                <label>Gender</label>
                <select v-model="sel.gender">
                    <option value="0">Male</option>
                    <option value="1">Female</option>
                </select>
            </p

            <p>
                <label>Patronymic</label>
                <input type="text" name="sel.patronymic" v-model="sel.patronymic" />            
            </p>

            <p>
                <label>Email</label>
                <input type="email" name="sel.email" v-model="sel.email" />
            </p>

            <p>
                <label>PhoneNumber</label>
                <input type="tel" name="sel.phoneNumber" v-model="sel.phoneNumber" />          
            </p>

            <p>
                <label>DateOfBirth</label>
                <input type="date" name="sel.dateOfBirth" v-model="sel.dateOfBirth" />
            </p>

            <button type="button" @click="changeContact(sel)"><i class="fa fa-check-circle"></i></button>
        </popup-dialog>


        <!--ADD-->
        <!--fail to create a separate component-->
        <popup-dialog v-bind:show.sync="isDisplayPopUpAdd">

            <p v-if="errors.length">
                <b>Errors:</b>
                <ul>
                    <li v-for="error in errors">{{ error }}</li>
                </ul>
            </p>

            <p>
                <label>Name</label>
                <input type="text" name="newContact.name" v-model="newContact.name" />
            </p>
           
            <p>
                <label>SurName</label>
                <input type="text" name="newContact.surName" v-model="newContact.surName" />    
            </p>

            <p>
                <label>Gender</label>
                <select v-model="newContact.gender">
                    <option value="0">Male</option>
                    <option value="1">Female</option>
                </select>
            </p>
           
            <p>
                <label>Patronymic</label>
                <input type="text" name="newContact.patronymic" v-model="newContact.patronymic" />   
            </p>
           
            <p>
                <label>Email</label>
                <input type="email" name="newContact.email" v-model="newContact.email" />
            </p>
           
            <p>
                <label>PhoneNumber</label>
                <input type="tel" name="newContact.phoneNumber" v-model="newContact.phoneNumber" />
            </p>
           
            <p>
                <label>DateOfBirth</label>
                <input type="date" name="newContact.dateOfBirth" v-model="newContact.dateOfBirth" />
            </p>

            <button type="button" @click="addContact(newContact)"><i class="fa fa-check-circle"></i></button>
        </popup-dialog>

        <ul class="contacts-box">
            <li><contact-label/></li>
            <li v-for="item in contacts" :key="item.id">              
                <contact @click.native="sel = {...item}" :item="item" ></contact>
                <button v-if="sel?.id == item.id" type="button" @click="removeContact(item)" ><i class="fa fa-trash"></i></button>
                <button v-if="sel?.id == item.id" type="button" @click="displayPopUpChange" ><i class="fa fa-edit"></i></button>
            </li>
        </ul>
    </div>

    <script>

        let app = new Vue({
            el: '#app',
            data: {
                newContact: {
                    name: '',
                    surName: '',
                    patronymic: '',
                    gender: 0,
                    email: '',
                    phoneNumber: '',
                    dateOfBirth: Date.now.toDateString
                },
                contacts: [],
                sel: {
                    name: String,
                    id: Number,
                    surName: String,
                    patronymic: String,
                    gender: Number,
                    email: String,
                    phoneNumber: String,
                    dateOfBirth: Date
                },
                isDisplayPopUpAdd: false,
                isDisplayPopUpChange: false,
                errors: []
            },

            methods: {

                getContacts: function () {
                    contactService.get().then(contacts => this.contacts = contacts);
                },

                addContact: function (item) { 

                    if (this.isValid(item) == false) {
                        return;
                    }

                    let contact = { ...item, id: 0 };
                    contact.id = contactService.add(contact);
                    this.contacts.push(contact);

                    for (var member in item) delete item[member];
                    this.isDisplayPopUpAdd = false;
                },

                removeContact: function (item) {
                    contactService.remove(item.id).then(x => {
                        this.contacts = this.contacts.filter(contact => contact !== item)
                    });
                },

                changeContact: function (updateContact) {

                    if (this.isValid(updateContact) == false) {
                        return;
                    }
                        
                    contactService.change(updateContact).then(
                        this.contacts = this.contacts.map(value => value.id == updateContact.id ? { ...updateContact } : value)
                    )

                    this.isDisplayPopUpChange = false;
                    updateContact.id = -1;
                },


                displayPopUpAdd: function () {
                    this.errors = [];
                    this.isDisplayPopUpAdd = true;
                },

                displayPopUpChange: function () {
                    this.errors = [];
                    this.isDisplayPopUpChange = true;
                },


                isValid: function (contact) {
                    this.errors = [];
                    if (this.validateEmail(contact.email) == false) {
                        this.errors.push('Not valid email address');
                    }
                    if (this.validatePhone(contact.phoneNumber) == false) {
                        this.errors.push('Not valid phone number');
                    }
                    if (contact.name === undefined || contact.name.length < 1) {
                        this.errors.push('Name too short');
                    }
                    if (contact.surName === undefined || contact.surName.length < 1) {
                        this.errors.push('Surname too short');
                    }
                    if (contact.patronymic === undefined || contact.patronymic.length < 1) {
                        this.errors.push('Patronymic too short');
                    }

                    return this.errors.length == 0;
                },
                validateEmail: function(email) {
                    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                    return re.test(email);
                },
                validatePhone: function (phone) {
                    const re = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/;
                    return re.test(phone);
                }

                    
            },
            mounted() {
                let style = document.createElement('link');
                style.type = "text/css";
                style.rel = "stylesheet";
                style.href = 'app/components/PopUpDialog.css';
                document.head.appendChild(style);
            },
            created: function () {
                this.getContacts();
            }
        })
    </script>
   
    
</asp:Content>
