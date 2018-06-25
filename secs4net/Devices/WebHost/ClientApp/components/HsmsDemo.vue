<template>
    <div>
        <el-container>
            <el-header>
                <el-row type="flex" :gutter="5" align="middle">
                    <el-col :span="2">
                        <div class="grid-content bg-purple">IP Address:</div>
                    </el-col>
                    <el-col :span="3">
                        <div class="grid-content bg-purple"><el-input v-model="ip" placeholder="IP address"></el-input></div>
                    </el-col>
                    <el-col :span="1">
                        <div class="grid-content bg-purple">Port</div>
                    </el-col>
                    <el-col :span="2">
                        <div class="grid-content bg-purple"><el-input v-model="port" placeholder="port number" type="number"></el-input></div>
                    </el-col>
                    <el-col :span="2">
                        <div class="grid-content bg-purple"><el-checkbox v-model="active">Active Mode</el-checkbox></div>
                    </el-col>
                    <el-col :span="3">
                        <el-button size="medium" @click="connect" :disabled="!canConnect">Connect</el-button>
                    </el-col>
                    <el-col :span="3">
                        <el-button size="medium" @click="disconnect" :disabled="canConnect">Disconnect</el-button>
                    </el-col>
                    <el-col :span="4">
                        Status: {{status}}
                    </el-col>
                </el-row>            
            </el-header>

            <el-main>
                <el-row type="flex" :gutter="10">
                    <el-col>
                        <el-row><el-col>Sending Primary Message:</el-col></el-row>
                        <el-row>
                            <el-col><el-input type="textarea" :autosize="{ minRows: 13, maxRows: 13}" v-model="primaryMessageForSend"></el-input></el-col>
                        </el-row>
                        <el-row>
                            <el-col><el-button @click="sendPrimaryMessage" :disabled="!primaryMessageForSend">Send</el-button></el-col>
                        </el-row>
                        <el-row><el-col>Received Secondary Message:</el-col></el-row>
                        <el-row>
                            <el-col><el-input type="textarea" :autosize="{ minRows: 13, maxRows: 13}" v-model="receivedSecondayMessage" readonly></el-input></el-col>
                        </el-row>
                    </el-col>
                    <el-col>
                        <el-row><el-col>Received Primary Message:</el-col></el-row>
                        <el-row :gutter="10">                            
                            <el-col :span="6">
                                <select v-model="selectedPrimaryMessage" size="20" style="width:100%">
                                    <option v-for="(value, index) in recievedPrimaryMessages" v-bind:key="value.id" v-bind:value="{index: index, primaryMessage: value}">
                                        {{ value.title }}
                                    </option>
                                </select>
                            </el-col>
                            <el-col :span="18"><el-input type="textarea" :autosize="{ minRows: 15, maxRows: 15}"  v-model="selectedPrimaryMessageText" readonly></el-input></el-col>
                        </el-row>
                        <el-row><el-col>Reply Secondary Message:</el-col></el-row>
                        <el-row>
                            <el-col><el-input type="textarea" :autosize="{ minRows: 13, maxRows: 13}" v-model="secondaryMessageForSend"></el-input></el-col>
                        </el-row>
                        <el-row>
                            <el-col><el-button @click="replyS9F7" :disabled="this.selectedPrimaryMessage === undefined">Reply S9F7</el-button></el-col>
                        </el-row>
                        <el-row>
                            <el-col><el-button @click="replySecondaryMessage" :disabled="!canReplyMessage">Reply</el-button></el-col>
                        </el-row>
                    </el-col>
                </el-row>
            </el-main>
        </el-container>        
    </div>    
</template>

<script lang="ts">
import Vue from 'vue';
import * as signalR from '@aspnet/signalr';
import { connect } from 'http2';

interface IVewModel {
    ip: string,
    port: number,
    active: boolean,
    status: string,        
    primaryMessageForSend: string,
    receivedSecondayMessage: string,
    selectedPrimaryMessage?: SelectedPrimaryMessage,
    secondaryMessageForSend: string,
    recievedPrimaryMessages: IReceivedPrimaryMessage[]
}

interface SelectedPrimaryMessage {
    index: number, 
    primaryMessage: IReceivedPrimaryMessage,
}

interface IReceivedPrimaryMessage {
    id: number,
    title: string,
    message: string,
}

const defaultStatus = 'N/A';
let connection: signalR.HubConnection | null = null;

export default Vue.extend({
    name: 'Demo',
    data(): IVewModel {
        return {
            ip: '127.0.0.1',
            port: 5000,
            active: false,
            status: defaultStatus,
            primaryMessageForSend: '',
            receivedSecondayMessage: '',
            selectedPrimaryMessage: undefined,
            secondaryMessageForSend: '',
            recievedPrimaryMessages: [],
        };
    },
    computed: {
        canConnect: function() {
            return this.status === defaultStatus
        },
        canReplyMessage: function(){
            return this.selectedPrimaryMessage !==undefined && this.secondaryMessageForSend;
        },
        selectedPrimaryMessageText: function() {
            return this.selectedPrimaryMessage !== undefined ? this.selectedPrimaryMessage.primaryMessage.message : '';
        },
    },
    methods: {
        async connect() {
            connection = new signalR.HubConnectionBuilder()
                .withUrl(`/secs?ipaddress=${this.ip}&port=${this.port}&active=${this.active}`)
                .configureLogging(signalR.LogLevel.Information)
                .build();
           
            connection.on('ConnectionChanged', (status) => {
                this.status = status;
            });

            connection.on('Debug', (msg) => console.debug(msg));
            connection.on('Info', (msg) => console.info(msg));
            connection.on('Warning', (msg) => console.warn(msg));
            connection.on('Error',  (msg) => console.error(msg));
            connection.on('MessageIn', (msg) => console.info(msg));
            connection.on('MessageOut', (msg) => console.info(msg));
            connection.on('PrimaryMessageReceived', (msgId, msgTitle, msg) => { 
                this.recievedPrimaryMessages.push({
                    id: msgId,
                    title: msgTitle,
                    message: msg,
                });
            });

            await connection.start();
        },
        disconnect() {
            if (connection) {
                connection.stop();
                this.status = defaultStatus;
            }
            this.recievedPrimaryMessages = [];
            this.selectedPrimaryMessage = undefined;
        },
        async sendPrimaryMessage() {
            if (connection && this.primaryMessageForSend) {
                this.receivedSecondayMessage = await connection.invoke('SendMessage', this.primaryMessageForSend);
            }
        },
        async replySecondaryMessage() {
            if(this.secondaryMessageForSend) {
                await this.replySecondardMessageAndRemoveSelectedMessage(this.secondaryMessageForSend);
            }
        },
        async replyS9F7() {
            await this.replySecondardMessageAndRemoveSelectedMessage(undefined);
        },
        async replySecondardMessageAndRemoveSelectedMessage(replyMessage: string | undefined) {
             if(connection && this.selectedPrimaryMessage !== undefined) {
                await connection.invoke('ReplyMessage', this.selectedPrimaryMessage.primaryMessage.id, null);
                this.recievedPrimaryMessages.splice(this.selectedPrimaryMessage.index, 1);
                this.selectedPrimaryMessage = undefined;
            }
        }
    },
});
</script>

<style>
</style>
