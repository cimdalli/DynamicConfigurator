import { ReducerBuilder } from 'redux-ts'
import { ShowLoading, HideLoading } from '../actions'


export interface LayoutState {
    asyncCount?: number;
}

export const layoutReducer = new ReducerBuilder<LayoutState>()

    .init({
        asyncCount: 0
    })

    .handle(ShowLoading, (state) => {
        let count = state.asyncCount;
        return {
            asyncCount: ++count
        };
    })

    .handle(HideLoading, (state) => {
        let count = state.asyncCount;
        return {
            asyncCount: --count
        };
    })

    .build();