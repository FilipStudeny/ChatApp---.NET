import {
	Avatar,
	Badge,
	Link,
	ListItem,
	ListItemButton,
	ListItemIcon,
	ListItemText,
	styled,
	Typography,
} from "@mui/material";
import React from "react";
import { Link as RouterLink } from "react-router-dom";

import SidePanel from "./SidePanel";

const StyledBadge = styled(Badge)(({ theme }) => ({
	"& .MuiBadge-badge": {
		backgroundColor: "#44b700",
		color: "#44b700",
		boxShadow: `0 0 0 2px ${theme.palette.background.paper}`,
		width: "15px",
		height: "15px",
		borderRadius: "50%",
	},
}));

const FriendsPanel: React.FunctionComponent = () => (
	<SidePanel>
		<ListItem disablePadding>
			<Link component={RouterLink} to='/' color='inherit' underline='none' sx={{ width: 1 }}>
				<ListItemButton>
					<ListItemIcon>
						<StyledBadge
							overlap='circular'
							anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
							variant='dot'
						>
							<Avatar
								alt='User image'
								src='/user_icon.png'
								sx={{ width: 50, height: 50, m: "auto", bgcolor: "white" }}
								variant='circular'
							/>
						</StyledBadge>
					</ListItemIcon>
					<ListItemText>
						<Typography align='center' variant='subtitle1' component='p'>
							Abelard Abel
						</Typography>
					</ListItemText>
				</ListItemButton>
			</Link>
		</ListItem>
	</SidePanel>
);

export default FriendsPanel;
