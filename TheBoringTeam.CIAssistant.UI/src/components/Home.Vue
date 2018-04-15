<template>
  <div class="handler">
    <div class="navbar">
      <div id="logOut">
        {{userName}}
        <at-button type="primary" id="logoutBtn" v-on:click="logOut" title="Logout"><i class="fas fa-sign-out-alt"></i></at-button>
      </div>
      <img src="../assets/download.png" height="70%"/>
    </div>
    <div class = "activityBar">
      <div class="activities">
        <h3>Timeline</h3>
        <hr>
        <br>
        <div id="timeline">
          <at-timeline>
            <at-timeline-item v-for="timeline in timelines">
              <p>{{timeline.description}}(Created:{{timeline.dateCreation}})</p>
            </at-timeline-item>
          </at-timeline>
        </div>
      </div>
    </div>
    <div id="mainChat">
      <div class="chatArea" id="chatArea" v-chat-scroll>
        <div class="messageZone" v-for="post in posts">
          <div class="messageItem">
            <div class="infoRow">
              <i class="far fa-user" style="font-size: 3em;"></i>
            </div>
            <div class="messageRow">
              <strong>{{post.from}}</strong>
              <br>
              <p>{{post.message}}</p>
            </div>
          </div>
          <br>
        </div>
        <div class="typing-indicator" id='animatorItem' style="display:none">
          <span></span>
          <span></span>
          <span></span>
        </div>
      </div>
      <!-- <input v-on:keyup.enter="sendRequest" id="chatInput" v-model="chatInput" placeholder="Please input" append-button> -->
      <div v-on:keyup.enter="sendRequest">
        <at-input id="chatInput" v-model="chatInput" placeholder="Command to asistant" append-button>
          <template slot="append">
            <span v-on:click="startRecording" id="microphoneIcon"><i class="fas fa-microphone" title="Click to enable voice input" ></i></span>
          </template>
        </at-input>
      </div>
    </div>
  </div>
</template>
<script>
import Vue from 'vue'
import VueChatScroll from 'vue-chat-scroll'
Vue.use(VueChatScroll)
// import moment from 'moment'

import axios from 'axios';
var auAudioRecorder = require("au-audio-recorder");
export default {
  name: 'handler',
  data () {
    return {
      chatInput: '',
      recording: false,
      userName: localStorage.getItem('name'),
      posts: [
      ],
      timelines: [
      ]
    }
  },
   beforeCreate(){
      if(localStorage.getItem('name') == null)
      {
        document.body.style.backgroundColor = '#fff';
        window.location.href = "/#/Login";
      }
    },
  beforeMount(){
    this.updateTimeline();
  },
  methods : {
    startRecording: function(){
      let obj = this;
      var recognition = new webkitSpeechRecognition();
      recognition.continuous = true;
      recognition.interimResults = true;
      recognition.lang = "en-US";
      document.getElementById('microphoneIcon').style.color = 'blue';

      recognition.onresult = function(event) {
        if(event && event.results && event.results[0].isFinal)
        {
          document.getElementById('microphoneIcon').style.color = '#9B9B9B';
          obj.chatInput = event.results[0][0].transcript;
          recognition.stop();
        }
      }
      recognition.start();
    },
    loadData: function(){
      var date = new Date(localStorage.getItem('expiration')).valueOf();
      var today = new Date().setHours(-8).valueOf();
      // if(today > date)
      //   this.logOut();

      var obj = this;
      let headerInfo = "Bearer " + localStorage.getItem('token');

      axios({method: 'GET',url: 'http://theboringteamciassistantapi20180414031506.azurewebsites.net/api/v1/assistant/actions',
      headers: {Authorization: headerInfo},
      data: {}}).then(function(response){
        obj.timelines = response.data;
        var options = {weekday: 'long', year:'numeric', month:'long', day:'numeric',hour:'numeric',minute:'numeric'};
        for (var i = 0; i < obj.timelines.length; i++) {
          obj.timelines[i]['dateCreation'] = new Date(obj.timelines[i]['dateCreation']).toLocaleDateString('en-GB', options);
        }
      })
      .catch(function(error){
            console.log(error);
          });
    },
    updateTimeline: function(){
      this.loadData();
      setInterval(function () {
        this.loadData();
      }.bind(this), 7000);
    },
    sendRequest: function(){
      let obj = this;
      this.posts.push({from: this.userName, message: this.chatInput});
      let el = document.getElementById('animatorItem');
      if(el)
        el.style.display = 'table';
      let request = this.chatInput;
      let sessionId = sessionStorage.getItem('id');
      let headerInfo = "Bearer " + localStorage.getItem('token');
      this.chatInput = "";

      axios({method: 'POST',url: 'https://theboringteamciassistantapi20180414031506.azurewebsites.net/api/v1/assistant',
      headers: {Authorization: headerInfo},
      data: {Sentence : request, SessionId: sessionId}}).then(function(response){
        sessionStorage.setItem('id', response.data['sessionId']);
        obj.posts.push({from: 'Bot', message: response.data['sentence']});
        el.style.display = 'none';
      })
      .catch(function(error){
            console.log(error);
            obj.posts.push({from: 'Bot', message: "Sorry, something went haywire."});
            el.style.display = 'none';
            sessionStorage.removeItem('id');
          });
    },
    logOut: function(){
      localStorage.removeItem("expiration");
      localStorage.removeItem("token");
      localStorage.removeItem("name");
      document.body.style.backgroundColor = '#fff';
      window.location.href = "/#/Login";
    }
  }
}

</script>

<style scoped>
  .activities{
    padding: 10px;
  }
  #timeline{
    overflow-y:auto;
    height: 76vh;
  }
  #logoutBtn{
    height: 28px;
    background-color: #6190E8;
  }
  #mainChat{
    width: 70%;
    padding:2%;
    background-color: #ffffff;
    height: 90vh;
  }
  .navbar{
    height: 10vh;
    background-color: #303030;
    padding-top: 10px;
    padding-left: 10px;
    padding-right: 10px;
    color: white;
  }
  #logOut{
    float: right;
    margin-top: 1%;
    font-size: 18px;
  }
  img{
    float: left;
  }
  .activityBar{
    width: 30%;
    float: right;
    height: 90vh;
    background-color: #ffffff;
    border-left: 6px solid #303030;
  }
  #chatInput{
    width:96%;
    margin-left: 2%;
    margin-top: 10px;
  }
  .chatArea{
    z-index: -1;
    width:96%;
    height: 80vh;
    margin-left: 2%;
    overflow-y:auto;
  }
  .messageItem{
    display: grid;
    border-radius: 8px;
    grid-template-columns: 1fr 9fr;
    background-color: white;
  }
  .messageItem:hover{
    display: grid;
    border-radius: 8px;
    grid-template-columns: 1fr 9fr;
    background-color: #eff0f1;
  }
  .messageRow{
    /* background-color:red; */
    text-align: left;
    font-size: 18px;
    padding: 10px;
    padding-left: 0;
  }
  .infoRow{
    /* background-color:green; */
    text-align: left;
    font-size: 14px;
    padding: 3px;
    padding-top: 10px;
    padding-left: 10px;
  }

</style>
