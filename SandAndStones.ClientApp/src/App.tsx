import './App.css';
import AuthProvider from './context/AuthProvider.tsx';
import Routes from './components/Routes.tsx';

function App() {
    return (
        <AuthProvider>
            <Routes />
        </AuthProvider>
    );
};

export default App;