import React, { use, useEffect } from 'react';
import logo from './logo.svg';
import './App.css';
import { useStore } from './stores';
import { QRCodeSVG } from 'qrcode.react';
import { observer } from 'mobx-react-lite';

function App() {
  const { signalrStore: { qrcodeData, createHubConnection, username } } = useStore();

  useEffect(() => {
    createHubConnection();
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />

        {!qrcodeData && (<div style={{ color: 'white', fontSize: '25px' }}>
          loading...
        </div>)}

        {
          qrcodeData && <div>
            <QRCodeSVG value={qrcodeData} />
          </div>
        }

        {username && <div style={{ color: 'white', fontSize: '25px' }}>
          Welcome, {username}!
        </div>}
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>

      <div>

      </div>
    </div>
  );
}

export default observer(App);
