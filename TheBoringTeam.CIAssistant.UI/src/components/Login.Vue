<template>
    <div id="loginSector">
      <img src=".././assets/download.png">
      <br/>
      <h1>Login here</h1>
      <br/>
      <at-alert id="errorHandler" v-bind:message='error' type="error" closable></at-alert>
      <at-input v-model="inputValue" placeholder="Username" id="login" name="login" autocomplete="off"></at-input>
      <at-input v-model="password" type="password" placeholder="Password" id="login" name="login"></at-input>
      <at-button type="primary" class="loginBtn" id="primaryLogin" v-on:click="tryLogin" size="large">Login</at-button>
      <at-button type="primary" class="loginBtn" id="loadingLogin" v-on:click="tryLogin" loading size="large"></at-button>
    </div>
</template>


<script>
import axios from 'axios';
export default {
  name: 'loginSector',
  data () {
    return {
      inputValue: '',
      password: '',
      error: ''
    }
  },
  beforeCreate(){
    if(localStorage.getItem('name') != null)
    {
      document.body.style.backgroundColor = '#303030';
      window.location.href = "/#/home";
    }
  },
  methods : {
      tryLogin : function(){
        document.getElementById('primaryLogin').style.display = "none";
        document.getElementById('loadingLogin').style.display = "inline-block";
        let item = ['val'];
        var obj = this;
        if(this.inputValue == "")
        {
          this.error = 'Please enter a username';
          document.getElementById('errorHandler').style.display = "none";
          document.getElementById('errorHandler').style.display = "flex";
          document.getElementById('loadingLogin').style.display = "none";
          document.getElementById('primaryLogin').style.display = "inline-block";
          return;
        }
        if(this.password == "")
        {
          this.error = 'Please enter a password';
          document.getElementById('errorHandler').style.display = "none";
          document.getElementById('errorHandler').style.display = "flex";
          document.getElementById('loadingLogin').style.display = "none";
          document.getElementById('primaryLogin').style.display = "inline-block";
          return;
        }
        var infoToSend = "Basic " + btoa(this.inputValue + ":" + this.password);
        axios({method: 'POST',url: 'https://theboringteamciassistantapi20180414031506.azurewebsites.net/api/v1/users/login',
        headers: {Authorization: infoToSend},
        data: {}})
      .then(function(response){
        console.log(response);
        localStorage.setItem("expiration", response.data['expiration']);
        localStorage.setItem("token", response.data['token']);
        localStorage.setItem("name", response.data['user']['name']);
        document.body.style.backgroundColor = '#303030';
        window.location.href = "/#/home";
      })
      .catch(function(error){
            obj.error = 'Invalid credentials.';
            document.getElementById('errorHandler').style.display = "none";
            document.getElementById('errorHandler').style.display = "flex";
            document.getElementById('loadingLogin').style.display = "none";
            document.getElementById('primaryLogin').style.display = "inline-block";
            console.log(error);
            return;
          });
      }
    }
}
</script>

<style scoped>

  h1{
    margin-bottom: 5px;
  }
  #login{
    width: 40%;
    margin-left: 30%;
    margin-bottom: 15px;
  }

  #loadingLogin{
    display: none;
  }

  #errorHandler{
    width: 40%;
    margin-left:30%;
    margin-top: 12px;
    margin-bottom: 12px;
    display: none;
  }
</style>
