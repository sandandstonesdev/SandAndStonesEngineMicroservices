import { UserProfile } from "../types/UserProfile";

export type UserProfileProps = {
    userProfile: UserProfile | undefined
}
function UserProfileInfo( { userProfile }: UserProfileProps) {
    return (
        <>
            <h4>Your user profile Info</h4>
                <div>
                    <label htmlFor="userName">User Name</label>
                    <input readOnly={true} value={userProfile?.userName} />
                </div>
                <div>
                    <label htmlFor="email">Email</label>
                    <input readOnly={true} value={userProfile?.email} />   
                </div>
                <div>
                    <label htmlFor="birthAt">Date Of Birth</label>
                    <input readOnly={true} value={userProfile?.birthAt.toString()} />   
                </div>
                <div>
                    <label htmlFor="createdAt">User Created At</label>
                    <input readOnly={true} value={userProfile?.createdAt.toString()} />   
                </div>
                <div>
                    <label htmlFor="priviledgeLevel">Priviledge Level</label>
                    <input readOnly={true} value={userProfile?.priviledgeLevel} />   
                </div>
                <div>
                    <label htmlFor="currentUserIP">Current User Ip</label>
                    <input readOnly={true} value={userProfile?.currentUserIP} />   
                </div>
                <div>
                    <label htmlFor="userAvatar">User Avatar File</label>
                    <input readOnly={true} value={userProfile?.userAvatar} />   
                </div>
        </>
  );
}

export default UserProfileInfo;