import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import axios from "axios";
import { makeAutoObservable, runInAction } from "mobx";


export default class SignalR {
    hubConnection: HubConnection | null = null;
    qrcodeData: string | null = null;
    username: string | null = null;

    constructor() {
        makeAutoObservable(this);
    }


    createHubConnection = () => {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5084/hubs/' + 'presence')
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        this.hubConnection.on('QrCodeConnect', (qrcodeData: string) => {
            runInAction(() => {
                console.log('Received QR code data:', qrcodeData);
                this.qrcodeData = qrcodeData;
            });
        })

        this.hubConnection.on('Login', async (username: string) => {

            runInAction(() => {
                console.log('Received username:', username);
            });

            const res = await axios.post(`http://localhost:5084/api/Account/login-signalr/${username}`)

            runInAction(() => {
                console.log('Login response:', res.data);
                this.username = res.data.userName;
            });
        })

        this.hubConnection.start().catch(error => console.log('Error establishing the connection: ', error));
    }
}