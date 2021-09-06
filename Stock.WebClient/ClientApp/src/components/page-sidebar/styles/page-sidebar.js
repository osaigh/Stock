import styled from "styled-components/macro";
import { Link as ReactRouterLink } from "react-router-dom";

export const NavLink = styled(ReactRouterLink)`
  color: white;
`;

export const AsideContainer = styled.aside`
  background-image: linear-gradient(
    to right,
    rgba(58, 59, 61, 1),
    rgba(83, 85, 89, 1)
  );
  width: 200px;
`;

export const UnOrderedList = styled.ul``;

export const ListItem = styled.li``;
