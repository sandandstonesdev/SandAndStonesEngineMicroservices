import { PropsWithChildren, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from './AuthProvider';
import { User } from '../types/User';
//import { useLocalStorage } from './AuthProvider';

type ProtectedRouteProps = PropsWithChildren;

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
    const navigate = useNavigate();
    const { getUser } = useUser();
    
    useEffect(() => {
        const user: User | "" = getUser();
        if (user === null || user === "") {
            navigate('/login', { replace: true });
        }
    }, [navigate, getUser]);

    return children;
}