import { createContext, useContext } from "react";
import SignalR from "./SignalR";


interface Store {
    signalrStore: SignalR;
}

export const store: Store = {
    signalrStore: new SignalR(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}