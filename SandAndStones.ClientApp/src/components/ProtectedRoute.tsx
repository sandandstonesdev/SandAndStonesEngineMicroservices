import { PropsWithChildren, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { User } from '../types/User';
import { useUser } from '../hooks/useUser';

type ProtectedRouteProps = PropsWithChildren;

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
    const navigate = useNavigate();
    const { getUser } = useUser();
    
    useEffect(() => {
        const user: User | null = getUser();
        if (user === null) {
            navigate('/login', { replace: true });
        }
    }, [navigate, getUser]);

    return children;
}