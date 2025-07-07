

import { SIGNUP_ACTIONS } from '/src/Store/Modules/storeConstants.js'

import axios from "axios"

export default{

 async [SIGNUP_ACTIONS](context,payload){
   try {
       await axios.post('http://localhost:5146/auth/signup',{
       username:payload.username,
       password:payload.password,
     },{withCredentials:true});
   }
   catch(error) {
   throw error}
 }
    };

