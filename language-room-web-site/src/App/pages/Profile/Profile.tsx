import { UserInfoCard } from '@features/UserInfoCard/UserInfoCard'
import ProfileIcon from '@assets/profile.svg'

export function Profile() {
    return(
        <>
        <div className='w-[1834px] my-0 mx-auto'> 
          <div className=''>
            <UserInfoCard />
          </div>
        </div>
        </>
    )
}