import './App.css';
import Routes from './components/Routes.tsx';
import { Provider } from 'react-redux';
import { store } from './redux/store/Store.tsx';

function App() {
    return (
        <Provider store={store}>
            <Routes />
        </Provider>
    );
};

export default App;